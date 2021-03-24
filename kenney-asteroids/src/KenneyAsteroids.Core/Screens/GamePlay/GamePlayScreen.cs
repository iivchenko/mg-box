using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Eventing.Eventing;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayScreen : GameScreen
    {
        private readonly IEventSystem _bus;
        private readonly IEntitySystem _entities;
        private readonly ICollisionSystem _collisions;
        private readonly IRepository<GameSettings> _settingsRepository;

        private Viewport _viewport;
        private EntityFactory _factory;
        private SpriteSheet _spriteSheet;
        private EnemySpawner _enemySpawner;

        public GamePlayScreen(IServiceProvider container)
            : base(container)
        {
            _entities = Container.GetService<IEntitySystem>();
            _bus = Container.GetService<IEventSystem>();
            _settingsRepository = Container.GetService<IRepository<GameSettings>>();

            var rules = new List<IRule>
            {
                new LazyRule<Ship, Asteroid>
                (
                    (_, __) => true,
                    (ship, _) => _entities.Remove(ship)
                ),
                new LazyRule<Projectile, Asteroid>
                (
                    (_, __) => true,
                    (projectile, asteroid) => _entities.Remove(projectile, asteroid)
                )
            };

            _collisions = new CollisionSystem(rules);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _viewport = ScreenManager.Game.GraphicsDevice.Viewport;
            _spriteSheet = ScreenManager.Game.Content.Load<SpriteSheet>("SpriteSheets/Asteroids.sheet");
            var font = ScreenManager.Game.Content.Load<SpriteFont>("Fonts/Default");
            _factory = new EntityFactory(_spriteSheet, ScreenManager.SpriteBatch, Container.GetService<IPublisher>());
            _enemySpawner = new EnemySpawner(_viewport, _factory, Container.GetService<IPublisher>());

            var ship = _factory.CreateShip(new Vector(_viewport.Width / 2.0f, _viewport.Height / 2.0f));
            var controller = new ShipPlayerKeyboardController(ship);

            _entities.Add(controller, ship);

            var settings = _settingsRepository.Read();

            if (settings.ToggleFramerate.Toggle)
            {
                _entities.Add(new FrameRate(font, _viewport, ScreenManager.SpriteBatch));
            }
        }

        public override void Update(GameTime time, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(time, otherScreenHasFocus, coveredByOtherScreen);

            _entities.SelectUpdatable().Iter(x => x.Update(time));
            _collisions.ApplyCollisions(_entities.SelectBodies());
            _entities.SelectBodies().Where(IsOutOfScreen).Iter(HandleOutOfScreenBodies);
            _enemySpawner.Update(time);
            _bus.Update(time);
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

        private void HandleOutOfScreenBodies(IBody body)
        {
            switch(body)
            {
                case Projectile projectile:
                    _entities.Remove(projectile);
                    break;
                default:
                    var x = body.Position.X;
                    var y = body.Position.Y;

                    if (body.Position.X + body.Width / 2.0f < 0)
                    {
                        x = _viewport.Width + body.Width / 2.0f;
                    }
                    else if (body.Position.X - body.Width / 2.0f > _viewport.Width)
                    {
                        x = 0 - body.Width / 2.0f;
                    }

                    if (body.Position.Y + body.Height / 2.0f < 0)
                    {
                        y = _viewport.Height + body.Height / 2.0f;
                    }
                    else if (body.Position.Y - body.Height / 2.0f > _viewport.Height)
                    {
                        y = 0 - body.Height / 2.0f;
                    }

                    body.Position = new Vector(x, y);
                    break;
            }
        }
    }
}
