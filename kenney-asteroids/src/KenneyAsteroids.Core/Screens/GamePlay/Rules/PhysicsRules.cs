using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Leaderboards;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Rules;
using KenneyAsteroids.Engine.Screens;
using System;
using System.Numerics;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Particles;
using System.Linq;
using KenneyAsteroids.Engine.Audio;

namespace KenneyAsteroids.Core.Screens.GamePlay.Rules
{
    public static class PhysicsRules
    {
        public static class WhenAsteroidCollidesPlayerShip
        {
            public static class AndPlayerShipHasEnoughLifes
            {
                public sealed class ThenReduceLifes : IRule<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
                {
                    private readonly GamePlayHud _hud;

                    public ThenReduceLifes(IEntitySystem entities)
                    {
                        _hud = entities.First(x => x is GamePlayHud) as GamePlayHud;
                    }

                    public bool ExecuteCondition(GamePlayEntitiesCollideEvent<Ship, Asteroid> @event) => _hud.Lifes > 0;

                    public void ExecuteAction(GamePlayEntitiesCollideEvent<Ship, Asteroid> @event) => _hud.Lifes--;
                }

                public sealed class ThenResetPlayersShip : IRule<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
                {
                    private readonly GamePlayHud _hud;
                    private readonly IViewport _viewport;

                    public ThenResetPlayersShip(
                        IEntitySystem entities,
                        IViewport viewport)
                    {
                        _hud = entities.First(x => x is GamePlayHud) as GamePlayHud;
                        _viewport = viewport;
                    }

                    public bool ExecuteCondition(GamePlayEntitiesCollideEvent<Ship, Asteroid> @event) => _hud.Lifes > 0;

                    public void ExecuteAction(GamePlayEntitiesCollideEvent<Ship, Asteroid> @event) => @event.Body1.Position = new Vector2(_viewport.Width / 2, _viewport.Height / 2);
                }
            }

            public static class AndPlayerShipDoesntHaveEnoughLifes
            {
                public sealed class ThenRemovePlayersShipFromTheGame : IRule<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
                {
                    private readonly GamePlayHud _hud;
                    private readonly IEntitySystem _entities;

                    public ThenRemovePlayersShipFromTheGame(IEntitySystem entities)
                    {
                        _hud = entities.First(x => x is GamePlayHud) as GamePlayHud;
                        _entities = entities;
                    }

                    public bool ExecuteCondition(GamePlayEntitiesCollideEvent<Ship, Asteroid> @event) => _hud.Lifes <= 0;

                    public void ExecuteAction(GamePlayEntitiesCollideEvent<Ship, Asteroid> @event) => _entities.Remove(@event.Body1);
                }

                public sealed class ThenGameOver : IRule<GamePlayEntitiesCollideEvent<Ship, Asteroid>>
                {
                    private readonly GamePlayHud _hud;
                    private readonly LeaderboardsManager _leaderBoard;

                    public ThenGameOver(
                        IEntitySystem entities,
                        LeaderboardsManager leaderBoard)
                    {
                        _hud = entities.First(x => x is GamePlayHud) as GamePlayHud;
                        _leaderBoard = leaderBoard;
                    }

                    public bool ExecuteCondition(GamePlayEntitiesCollideEvent<Ship, Asteroid> @event) => _hud.Lifes <= 0;

                    public void ExecuteAction(GamePlayEntitiesCollideEvent<Ship, Asteroid> @event)
                    {
                        var ship = @event.Body1;

                        var playedTime = DateTime.Now - _hud.StartTime;

                        if (_leaderBoard.CanAddLeader(_hud.Scores))
                        {
                            var newHigthScorePrompt = new PromptScreen("Congratulations, you made new high score!\nEnter you name:");

                            newHigthScorePrompt.Accepted += (_, __) =>
                            {
                                _leaderBoard.AddLeader(newHigthScorePrompt.Text, _hud.Scores, playedTime);
                                GameOverMessage();
                            };
                            newHigthScorePrompt.Cancelled += (_, __) => GameOverMessage();

                            GameRoot.ScreenManager.AddScreen(newHigthScorePrompt, null);
                        }
                        else
                        {
                            GameOverMessage();
                        }
                    }

                    private void GameOverMessage()
                    {
                        const string message = "GAME OVER?\nA button, Space, Enter = Restart\nB button, Esc = Exit";
                        var msg = new MessageBoxScreen(message);

                        msg.Accepted += (_, __) => LoadingScreen.Load(GameRoot.ScreenManager, false, null, new StarScreen(), new GamePlayScreen());
                        msg.Cancelled += (_, __) => LoadingScreen.Load(GameRoot.ScreenManager, false, null, new StarScreen(), new MainMenuScreen());

                        GameRoot.ScreenManager.AddScreen(msg, null);
                    }
                }
            }
        }

        public static class WhenPlayersProjectileCollidesAsteroid
        {
            public sealed class ThenScore : IRule<GamePlayEntitiesCollideEvent<Projectile, Asteroid>>
            {
                private readonly GamePlayHud _hud;
                private readonly GamePlayScoreManager _scores;

                public ThenScore(IEntitySystem entities)
                {
                    _hud = entities.First(x => x is GamePlayHud) as GamePlayHud;

                    _scores = new GamePlayScoreManager();
                }

                public bool ExecuteCondition(GamePlayEntitiesCollideEvent<Projectile, Asteroid> @event) => true;

                public void ExecuteAction(GamePlayEntitiesCollideEvent<Projectile, Asteroid> @event)
                {
                    _hud.Scores += _scores.GetScore(@event.Body2);
                }
            }

            public sealed class ThenRemoveBoth : IRule<GamePlayEntitiesCollideEvent<Projectile, Asteroid>>
            {
                private readonly IEntitySystem _entities;
                private readonly IEventPublisher _publisher;

                public ThenRemoveBoth(
                   IEntitySystem entities,
                   IEventPublisher publisher)
                {
                    _entities = entities;
                    _publisher = publisher;
                }

                public bool ExecuteCondition(GamePlayEntitiesCollideEvent<Projectile, Asteroid> @event) => true;

                public void ExecuteAction(GamePlayEntitiesCollideEvent<Projectile, Asteroid> @event)
                {
                    var projectile = @event.Body1;
                    var asteroid = @event.Body2;

                    _entities.Remove(projectile, asteroid);

                    //_publisher.Publish(new EntityDestroyedEvent(projectile));
                    //_publisher.Publish(new EntityDestroyedEvent(asteroid));
                }
            }

            public sealed class ThenMakeAsteroidDeathEffect : IRule<GamePlayEntitiesCollideEvent<Projectile, Asteroid>>
            {
                private readonly IEntitySystem _entities;
                private readonly IEventPublisher _publisher;
                private readonly IPainter _painter;
                private readonly IAudioPlayer _player;
                private readonly IContentProvider _content;

                public ThenMakeAsteroidDeathEffect(
                   IEntitySystem entities,
                   IEventPublisher publisher,
                   IPainter painter,
                   IAudioPlayer player,
                   IContentProvider content)
                {
                    _entities = entities;
                    _publisher = publisher;
                    _painter = painter;
                    _player = player;
                    _content = content;
                }

                public bool ExecuteCondition(GamePlayEntitiesCollideEvent<Projectile, Asteroid> @event) => true;

                public void ExecuteAction(GamePlayEntitiesCollideEvent<Projectile, Asteroid> @event)
                {
                    var asteroid = @event.Body2;

                    var asteroids = _content.Load<SpriteSheet>("SpriteSheets/Asteroids.sheet");
                    var particles =
                        Particles
                        .CreateNew()
                        .WithInit(rand =>
                            Enumerable
                                .Range(0, rand.Next(5, 10))
                                .Select(_ =>
                                    new Particle
                                    {
                                        Angle = rand.Next(0, 360).AsRadians(),
                                        AngularVelocity = rand.Next(5, 100).AsRadians(),
                                        Color = Colors.White,
                                        Position = asteroid.Position,
                                        Scale = Vector2.One,
                                        Sprite = asteroids["meteorBrown_tiny1"],
                                        TTL = 4,
                                        Velocity = new Vector2(rand.Next(-100, 100), rand.Next(-100, 100))
                                    }))
                                .WithUpdate((rand, time, particle) =>
                                {
                                    particle.Position += particle.Velocity * time;
                                    particle.Angle += particle.AngularVelocity * time;
                                    particle.TTL -= time;
                                    particle.Color *= 0.99f;
                                    particle.Color = new Color(
                                    ClampColor(particle.Color.Red, time),
                                    ClampColor(particle.Color.Green, time),
                                    ClampColor(particle.Color.Blue, time),
                                    ClampColor(particle.Color.Alpha, time));
                                })
                                .Build((int)DateTime.Now.Ticks, _painter, _publisher);

                    _entities.Add(particles);

                    var explosion = _content.Load<Sound>("Sounds/asteroid-explosion.sound");

                    _player.Play(explosion);
                }

            private static byte ClampColor(byte color, float time)
                {
                    return Math.Clamp((byte)(color - color * 1.5 * time), (byte)0, (byte)255);
                }
            }
            public static class AndAsteroidIsBig
            {
                public sealed class TheFallAsteroidAppart : IRule<GamePlayEntitiesCollideEvent<Projectile, Asteroid>>
                {
                    private readonly IEntitySystem _entities;
                    private readonly IEntityFactory _entityFactory;

                    public TheFallAsteroidAppart(
                       IEntitySystem entities,
                       IEntityFactory entityFactory)
                    {
                        _entities = entities;
                        _entityFactory = entityFactory;
                    }

                    public bool ExecuteCondition(GamePlayEntitiesCollideEvent<Projectile, Asteroid> @event) => @event.Body2.Type == AsteroidType.Big;

                    public void ExecuteAction(GamePlayEntitiesCollideEvent<Projectile, Asteroid> @event)
                    {
                        var asteroid = @event.Body2;
                        var direction1 = asteroid.Velocity.ToRotation() - 20.AsRadians();
                        var direction2 = asteroid.Velocity.ToRotation() + 20.AsRadians();
                        var position1 = asteroid.Position;
                        var position2 = asteroid.Position;
                        var med1 = _entityFactory.CreateAsteroid(AsteroidType.Medium, position1, direction1);
                        var med2 = _entityFactory.CreateAsteroid(AsteroidType.Medium, position2, direction2);

                        _entities.Add(med1, med2);
                    }
                }
            }
        }
    }
}
