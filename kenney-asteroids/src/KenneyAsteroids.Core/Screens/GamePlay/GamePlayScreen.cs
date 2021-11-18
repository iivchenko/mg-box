using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Rules;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Numerics;

using XTime = Microsoft.Xna.Framework.GameTime;
using XMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayScreen : GameScreen
    {
        private IGameRuleSystem _rules;
        private IEntitySystem _entities;
        private ICollisionSystem _collisions;
        private IViewport _viewport;
        private IEventPublisher _publisher;

        private GamePlayDirector _director;
        private ShipPlayerController _controller;

        public override void Initialize()
        {
            base.Initialize();
            var container = ScreenManager.Container;

            _entities = container.GetService<IEntitySystem>();
            _rules = container.GetService<IGameRuleSystem>();
            _viewport = container.GetService<IViewport>();
            _publisher = container.GetService<IEventPublisher>();

            var factory = container.GetService<IEntityFactory>();
            var ship = factory.CreateShip(new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f));

            _collisions = new CollisionSystem();
            _director = new GamePlayDirector(
                container.GetService<IOptionsMonitor<AudioSettings>>(),
                container.GetService<IContentProvider>());

            _controller = new ShipPlayerController(ship);

            var timer1 = new Timer(TimeSpan.FromSeconds(3), GameTags.NextAsteroid, _publisher);
            var timer2 = new Timer(TimeSpan.FromSeconds(60), GameTags.NextAsteroidLimitChange, _publisher);
            var timer3 = new Timer(TimeSpan.FromSeconds(45), GameTags.NextHasardSituation, _publisher);
            var hud = new GamePlayHud(
                container.GetService<IOptionsMonitor<GameSettings>>(),
                container.GetService<IViewport>(),
                container.GetService<IPainter>(),
                container.GetService<IContentProvider>(),
                container.GetService<IFontService>());

            _entities.Add(
                ship, 
                hud,
                timer1,
                timer2,
                timer3);
        }

        public override void Free()
        {
            _director.Free();
            _entities.Remove(_entities.ToArray());
            _entities.Commit();

            XMediaPlayer.Stop();

            base.Free();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (input.IsNewKeyPress(Keys.Escape, null, out _) || input.IsNewButtonPress(Buttons.Start, null, out _))
            {
                XMediaPlayer.Pause();
                const string message = "Exit game?\nA button, Space, Enter = ok\nB button, Esc = cancel";
                var confirmExitMessageBox = new MessageBoxScreen(message);

                confirmExitMessageBox.Accepted += (_, __) => LoadingScreen.Load(ScreenManager, false, null, new StarScreen(), new MainMenuScreen());
                confirmExitMessageBox.Cancelled += (_, __) => XMediaPlayer.Resume();

                ScreenManager.AddScreen(confirmExitMessageBox, null);
            }

            _controller.Handle(input);
        }

        public override void Update(XTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!otherScreenHasFocus)
            {
                _entities
                    .Where(x => x is IUpdatable)
                    .Cast<IUpdatable>()
                    .Iter(x => x.Update(time));

                var bodies = _entities.Where(x => x is IBody).Cast<IBody>();

                // Move Collision eventing into collision system
                foreach(var collision in _collisions.EvaluateCollisions(bodies))
                {
                    switch((collision.Body1, collision.Body2))
                    {
                        case (Ship ship, Asteroid asteroid):
                            _publisher.Publish(new GamePlayEntitiesCollideEvent<Ship, Asteroid>(ship, asteroid));
                            break;

                        case (Asteroid asteroid, Ship ship):
                            _publisher.Publish(new GamePlayEntitiesCollideEvent<Ship, Asteroid>(ship, asteroid));
                            break;

                        case (Projectile projectile, Asteroid asteroid):
                            _publisher.Publish(new GamePlayEntitiesCollideEvent<Projectile, Asteroid>(projectile, asteroid));
                            break;

                        case (Asteroid asteroid, Projectile projectile):
                            _publisher.Publish(new GamePlayEntitiesCollideEvent<Projectile, Asteroid>(projectile, asteroid));
                            break;
                    }
                }

                // TODO: Make this rules
                bodies
                    .Where(IsOutOfScreen)
                    .Iter(HandleOutOfScreenBodies);

                _director.Update(time);
                _rules.Update(time);

                _entities.Commit();
            }
        }

        public override void Draw(XTime gameTime)
        {
            base.Draw(gameTime);

            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
    }
}
