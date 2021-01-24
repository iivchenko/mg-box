using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class GamePlayScreen : GameScreen
    {
        private readonly ICollisionSystem _collisions;
        private readonly Random _random;

        private Viewport _viewport;
        private SpriteSheet _spriteSheet;
        private EntityFactory _factory;
        private EntityCollection _entities;

        public GamePlayScreen()
        {
            _random = new Random();

            var rules = new List<IRule>
            {
                new LazyRule<Ship, Asteroid>
                (
                    (_, __) => true,
                    (ship, _) => _entities.Remove(ship)
                )
            };

            _collisions = new CollisionSystem(rules);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _viewport = ScreenManager.Game.GraphicsDevice.Viewport;
            _spriteSheet = ScreenManager.Game.Content.Load<SpriteSheet>("SpriteSheets/Asteroids.sheet");
            _factory = new EntityFactory(_spriteSheet, ScreenManager.SpriteBatch);

            var timer = new Timer(TimeSpan.FromSeconds(5), SpawnAsteroid, true);
            var ship = _factory.CreateShip(new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f));
            var controller = new ShipPlayerKeyboardController(ship);

            _entities = new EntityCollection
            {
                timer,
                controller,
                ship
            };

            timer.Start();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            _entities.SelectUpdatable().Iter(x => x.Update(gameTime));
            _collisions.ApplyCollisions(_entities.SelectBodies());
            _entities.SelectBodies().Where(IsOutOfScreen).Iter(Teleport);

            _entities.Commit();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();

            _entities.SelectDrawable().Iter(x => x.Draw(gameTime));

            ScreenManager.SpriteBatch.End();
        }

        private bool IsOutOfScreen(IBody entity)
        {
            return
                entity.Position.X + entity.Width / 2.0 < 0 ||
                entity.Position.X - entity.Width / 2.0 > _viewport.Width ||
                entity.Position.Y + entity.Height / 2.0 < 0 ||
                entity.Position.Y - entity.Height / 2.0 > _viewport.Height;
        }

        private void Teleport(IBody entity)
        {
            var x = entity.Position.X;
            var y = entity.Position.Y;

            if (entity.Position.X + entity.Width / 2.0f < 0)
            {
                x = _viewport.Width + entity.Width / 2.0f;
            }
            else if (entity.Position.X - entity.Width / 2.0f > _viewport.Width)
            {
                x = 0 - entity.Width / 2.0f;
            }

            if (entity.Position.Y + entity.Height / 2.0f < 0)
            {
                y = _viewport.Height + entity.Height / 2.0f;
            }
            else if (entity.Position.Y - entity.Height / 2.0f > _viewport.Height)
            {
                y = 0 - entity.Height / 2.0f;
            }

            entity.Position = new Vector2(x, y);
        }

        private void SpawnAsteroid(GameTime gameTime)
        {
            const int BigAsteroidMinSpeed = 15;
            const int BigAsteroidMaxSpeed = 100;
            const int BigAsteroidMinRotationSpeed = 5;
            const int BigAsteroidMaxRotationSpeed = 25;

            var x = 0;
            var y = 0;
            var dx = 0;
            var dy = 0;

            switch (_random.Next(0, 4))
            {
                case 0: // Up -> Down
                    x = _random.Next(0, _viewport.Width);
                    y = 0;
                    dx = _random.Next(0, _viewport.Width);
                    dy = _viewport.Height;
                    break;

                case 1: // Right -> Left
                    x = _viewport.Width;
                    y = _random.Next(0, _viewport.Height);
                    dx = 0;
                    dy = _random.Next(0, _viewport.Height);
                    break;

                case 2: // Down -> UP
                    x = _random.Next(0, _viewport.Width);
                    y = _viewport.Height;
                    dx = _random.Next(0, _viewport.Width);
                    dy = 0;
                    break;

                case 3: // Left -> Right
                    x = 0;
                    y = _random.Next(0, _viewport.Height);
                    dx = _viewport.Width;
                    dy = _random.Next(0, _viewport.Height);
                    break;
            }

            var position = new Vector2(x, y);
            var direction = new Vector2(dx, dy) - position;
            direction.Normalize();

            var velocity = direction * new Vector2(_random.Next(BigAsteroidMinSpeed, BigAsteroidMaxSpeed), _random.Next(BigAsteroidMinSpeed, BigAsteroidMaxSpeed));
            var rotationSpeed = MathHelper.ToRadians(_random.Next(BigAsteroidMinRotationSpeed, BigAsteroidMaxRotationSpeed)) * _random.NextDouble() > 0.5 ? 1 : -1;

            var asteroid = _factory.CreateAsteroid(position, velocity, rotationSpeed);

            _entities.Add(asteroid);
        }
    }
}
