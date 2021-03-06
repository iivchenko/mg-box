﻿using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Messaging;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using XTime = Microsoft.Xna.Framework.GameTime;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayScreen : GameScreen
    {
        private IMessageSystem _bus;
        private IEntitySystem _entities;
        private ICollisionSystem _collisions;
        private IViewport _viewport;

        private GamePlayHud _hud;
        private GamePlayDirector _director;
        private ShipPlayerController _controller;

        public override void Initialize()
        {
            base.Initialize();

            _entities = ScreenManager.Container.GetService<IEntitySystem>();
            _bus = ScreenManager.Container.GetService<IMessageSystem>();
            _viewport = ScreenManager.Container.GetService<IViewport>();

            var publisher = ScreenManager.Container.GetService<IPublisher>();
            var factory = ScreenManager.Container.GetService<IEntityFactory>();

            var rules = new List<IRule>
            {
                new LazyRule<Ship, Asteroid>
                (
                    (_, __) => _hud.Lifes > 0,
                    (ship, _) => RestartShip(ship)
                ),
                new LazyRule<Ship, Asteroid>
                (
                    (_, __) => _hud.Lifes <= 0,
                    (ship, _) => GameOver(ship)
                ),
                new LazyRule<Projectile, Asteroid>
                (
                    (_, __) => true,
                    (projectile, asteroid) =>
                    {
                        _entities.Remove(projectile, asteroid);
                        publisher.Publish(new EntityDestroyedEvent(asteroid));
                    }
                )
            };

            var ship = factory.CreateShip(new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f));

            _collisions = new CollisionSystem(rules);
            _director = new GamePlayDirector(publisher, ScreenManager.Container.GetService<IOptionsMonitor<AudioSettings>>(), ScreenManager.Container.GetService<ContentManager>());
            _controller = new ShipPlayerController(ship);
            _hud = new GamePlayHud(ScreenManager.Container);

            _entities.Add(ship, _hud);
        }

        public override void Free()
        {
            _director.Free();
            _entities.Remove(_entities.ToArray());
            _entities.Commit();
            
            MediaPlayer.Stop();

            base.Free();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (input.IsNewKeyPress(Keys.Escape, null, out _) || input.IsNewButtonPress(Buttons.Start, null, out _))
            {
                MediaPlayer.Pause();
                const string message = "Exit game?\nA button, Space, Enter = ok\nB button, Esc = cancel";
                var confirmExitMessageBox = new MessageBoxScreen(message);

                confirmExitMessageBox.Accepted += (_, __) => LoadingScreen.Load(ScreenManager, false, null, new MainMenuScreen());
                confirmExitMessageBox.Cancelled += (_, __) => MediaPlayer.Resume();

                ScreenManager.AddScreen(confirmExitMessageBox, null);
            }

            _controller.Handle(input);
        }

        public override void Update(XTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            var time = gameTime.ToDelta();

            if (!otherScreenHasFocus)
            {
                _entities
                    .Where(x => x is IUpdatable)
                    .Cast<IUpdatable>()
                    .Iter(x => x.Update(time));

                var bodies = _entities.Where(x => x is IBody).Cast<IBody>();

                _collisions.ApplyCollisions(bodies);

                bodies
                    .Where(IsOutOfScreen)
                    .Iter(HandleOutOfScreenBodies);

                _director.Update(time);
                _bus.Update(time);

                _entities.Commit();
            }
        }

        public override void Draw(XTime gameTime)
        {
            base.Draw(gameTime);

            var time = gameTime.ToDelta();

            _entities.Where(x => x is IDrawable).Cast<IDrawable>().Iter(x => x.Draw(time));
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
            switch (body)
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

                    body.Position = new Vector2(x, y);
                    break;
            }
        }

        private void RestartShip(Ship ship)
        {
            _hud.Lifes--;
            ship.Position = new Vector2(_viewport.Width / 2, _viewport.Height / 2);
        }

        private void GameOver(Ship ship)
        {
            _entities.Remove(ship);

            const string message = "GAME OVER?\nA button, Space, Enter = Restart\nB button, Esc = Exit";
            var msg = new MessageBoxScreen(message);

            msg.Accepted += (_, __) => LoadingScreen.Load(ScreenManager, false, null, new GamePlayScreen());
            msg.Cancelled += (_, __) => LoadingScreen.Load(ScreenManager, false, null, new MainMenuScreen());

            ScreenManager.AddScreen(msg, null);
        }
    }
}
