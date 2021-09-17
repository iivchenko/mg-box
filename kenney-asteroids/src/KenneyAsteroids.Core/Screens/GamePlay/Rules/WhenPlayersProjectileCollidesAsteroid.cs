﻿using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Rules;
using KenneyAsteroids.Engine.Particles;
using System;
using System.Linq;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay.Rules
{
    public static class WhenPlayersProjectileCollidesAsteroid
    {
        public sealed class ThenScore : IRule<GamePlayEntitiesCollideEvent<Projectile, Asteroid>>
        {
            private readonly GamePlayHud _hud;
            private readonly GamePlayScoreManager _scores;

            public ThenScore(GamePlayHud hud)
            {
                _hud = hud;

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

                _publisher.Publish(new EntityDestroyedEvent(projectile));
                _publisher.Publish(new EntityDestroyedEvent(asteroid));
            }
        }

        public sealed class ThenMakeAsteroidDeathEffect : IRule<GamePlayEntitiesCollideEvent<Projectile, Asteroid>>
        {
            private readonly IEntitySystem _entities;
            private readonly IEventPublisher _publisher;
            private readonly IPainter _painter;
            private readonly IContentProvider _content;

            public ThenMakeAsteroidDeathEffect(
               IEntitySystem entities,
               IEventPublisher publisher,
               IPainter painter,
               IContentProvider content)
            {
                _entities = entities;
                _publisher = publisher;
                _painter = painter;
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
