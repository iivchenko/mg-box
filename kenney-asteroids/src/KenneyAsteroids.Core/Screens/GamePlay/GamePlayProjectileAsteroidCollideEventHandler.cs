using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Messaging;
using KenneyAsteroids.Engine.Particles;
using System;
using System.Linq;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayProjectileAsteroidCollideEventHandler :
       IMessageHandler<GamePlayEntitiesCollideEvent<Projectile, Asteroid>>
    {
        private readonly GamePlayHud _hud;
        private readonly IEntitySystem _entities;
        private readonly IPublisher _publisher;
        private readonly IPainter _painter;
        private readonly IContentProvider _content;
        private readonly IEntityFactory _entityFactory;
        private readonly GamePlayScoreManager _scores;
        private readonly Random _random;

        public GamePlayProjectileAsteroidCollideEventHandler(
           GamePlayHud hud,
           IEntitySystem entities,
           IPublisher publisher,
           IPainter painter,
           IContentProvider content,
           IEntityFactory entityFactory)
        {
            _hud = hud;
            _entities = entities;
            _publisher = publisher;
            _painter = painter;
            _content = content;
            _entityFactory = entityFactory;

            _scores = new GamePlayScoreManager();
            _random = new Random();
        }

        public void Handle(GamePlayEntitiesCollideEvent<Projectile, Asteroid> message)
        {
            var projectile = message.Body1;
            var asteroid = message.Body2;

            _entities.Remove(projectile, asteroid);
            _hud.Scores += _scores.GetScore(asteroid);

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

            var direction1 = asteroid.Velocity.ToRotation() - 20.AsRadians();
            var direction2 = asteroid.Velocity.ToRotation() + 20.AsRadians();
            var position1 = asteroid.Position;
            var position2 = asteroid.Position;

            switch (asteroid.Type)
            {
                case AsteroidType.Big:
                    var med1 = _entityFactory.CreateAsteroid(AsteroidType.Medium, position1, direction1);
                    var med2 = _entityFactory.CreateAsteroid(AsteroidType.Medium, position2, direction2);
                    _entities.Add(med1, med2);

                    break;
                case AsteroidType.Medium:
                    var smal1 = _entityFactory.CreateAsteroid(AsteroidType.Small, position1, direction1);
                    var smal2 = _entityFactory.CreateAsteroid(AsteroidType.Small, position2, direction2);
                    _entities.Add(smal1, smal2);
                    break;

                case AsteroidType.Small:
                    var tiny1 = _entityFactory.CreateAsteroid(AsteroidType.Tiny, position1, direction1);
                    var tiny2 = _entityFactory.CreateAsteroid(AsteroidType.Tiny, position2, direction2);
                    _entities.Add(tiny1, tiny2);
                    break;

                default:
                    break;
            }

            _publisher.Publish(new EntityDestroyedEvent(asteroid));
        }

        private static byte ClampColor(byte color, float time)
        {
            return Math.Clamp((byte)(color - color * 1.5 * time), (byte)0, (byte)255);
        }
    }
}
