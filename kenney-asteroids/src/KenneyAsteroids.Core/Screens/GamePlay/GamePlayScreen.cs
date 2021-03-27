using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Eventing.Eventing;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private EnemySpawner _enemySpawner;
        private ShipPlayerKeyboardController _controller;

        public GamePlayScreen(IServiceProvider container)
            : base(container)
        {
            _entities = Container.GetService<IEntitySystem>();
            _bus = Container.GetService<IEventSystem>();

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

        public override void Initialize()
        {
            base.Initialize();

            var draw = Container.GetService<IPainter>();
            var publisher = Container.GetService<IPublisher>();
            var spriteSheet = Content.Load<SpriteSheet>("SpriteSheets/Asteroids.sheet");
            var factory = new EntityFactory(spriteSheet, publisher, draw);

            _enemySpawner = new EnemySpawner(GraphicsDevice.Viewport, factory, publisher);

            var ship = factory.CreateShip(new Vector2(GraphicsDevice.Viewport.Width / 2.0f, GraphicsDevice.Viewport.Height / 2.0f));
            _controller = new ShipPlayerKeyboardController(ship);

            _entities.Add(ship, new GamePlayHud(Container));
        }

        public override void Free()
        {
            _entities.Remove(_entities.ToArray());

            base.Free();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (input.IsNewKeyPress(Keys.Escape, null, out _))
            {
                const string message = "Exit game?\nA button, Space, Enter = ok\nB button, Esc = cancel";
                MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message, Container);

                confirmExitMessageBox.Accepted += (_, __) => LoadingScreen.Load(ScreenSystem, true, null, Container, new MainMenuScreen(Container));

                ScreenSystem.Add(confirmExitMessageBox, null);
            }

            _controller.Handle(input);
        }

        public override void Update(float time, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(time, otherScreenHasFocus, coveredByOtherScreen);

            if (!otherScreenHasFocus)
            {
                _entities.SelectUpdatable().Iter(x => x.Update(time));
                _collisions.ApplyCollisions(_entities.SelectBodies());
                _entities.SelectBodies().Where(IsOutOfScreen).Iter(HandleOutOfScreenBodies);
                _enemySpawner.Update(time);
                _bus.Update(time);
                _entities.Commit();
            }
        }

        public override void Draw(float time)
        {
            base.Draw(time);

            _entities.SelectDrawable().Iter(x => x.Draw(time));
        }

        private bool IsOutOfScreen(IBody entity)
        {
            return
                entity.Position.X + entity.Width / 2.0 < 0 ||
                entity.Position.X - entity.Width / 2.0 > GraphicsDevice.Viewport.Width ||
                entity.Position.Y + entity.Height / 2.0 < 0 ||
                entity.Position.Y - entity.Height / 2.0 > GraphicsDevice.Viewport.Height;
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
                        x = GraphicsDevice.Viewport.Width + body.Width / 2.0f;
                    }
                    else if (body.Position.X - body.Width / 2.0f > GraphicsDevice.Viewport.Width)
                    {
                        x = 0 - body.Width / 2.0f;
                    }

                    if (body.Position.Y + body.Height / 2.0f < 0)
                    {
                        y = GraphicsDevice.Viewport.Height + body.Height / 2.0f;
                    }
                    else if (body.Position.Y - body.Height / 2.0f > GraphicsDevice.Viewport.Height)
                    {
                        y = 0 - body.Height / 2.0f;
                    }

                    body.Position = new Vector2(x, y);
                    break;
            }
        }
    }
}
