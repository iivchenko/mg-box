using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace KenneyAsteroids.Engine.Particles
{
    public sealed class ParticleEngine : IEntity, IUpdatable, IDrawable
    {
        private readonly IList<Particle> _particles;
        private readonly Action<Random, float, Particle> _update;
        private readonly IPainter _painter;
        private readonly IEventPublisher _publisher;
        private readonly Random _random;

        private bool _isFinished = false;

        public ParticleEngine(
            IEnumerable<Particle> particles,
            Action<Random, float, Particle> update,
            int seed,
            IPainter painter,
            IEventPublisher publisher)
        {
            _particles = particles.ToList();
            _update = update;
            _painter = painter;
            _publisher = publisher;

            _random = new Random(seed);
        }

        public IEnumerable<string> Tags => Enumerable.Empty<string>();

        public void Update(float time)
        {
            if (!_isFinished)
            {
                _particles
                .Where(particle => particle.TTL > 0)
                .Iter(particle => _update(_random, time, particle));

                if (_particles.All(x => x.TTL <= 0))
                {
                    _isFinished = true;
                    _publisher.Publish(new ParticlesEmmisionFinishedEvent(this));
                }
            }            
        }

        public void Draw(float time)
        {
            _particles
                .Where(particle => particle.TTL > 0)
                .Iter(particle => 
                        _painter.Draw(
                            particle.Sprite, 
                            particle.Position, 
                            Vector2.Zero, 
                            particle.Scale, 
                            particle.Angle, 
                            particle.Color));
        }
    }
}
