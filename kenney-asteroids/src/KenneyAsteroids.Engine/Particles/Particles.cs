using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Engine.Particles
{
    public sealed class Particles
    {
        private Func<Random, IEnumerable<Particle>> _init;
        private Action<Random, float, Particle> _update;

        private Particles()
        {
            _init = _ => Enumerable.Empty<Particle>();
            _update = (_, __, ___) => { };
        }

        public static Particles CreateNew()
        {
            return new Particles();
        }

        public Particles WithInit(Func<Random, IEnumerable<Particle>> init)
        {
            _init = init;

            return this;
        }

        public Particles WithUpdate(Action<Random, float, Particle> update)
        {
            _update = update;

            return this;
        }

        public ParticleEngine Build(int seed, IPainter painter, IEventPublisher publisher)
        {
            return new ParticleEngine(_init(new Random(seed)), _update, seed, painter, publisher);
        }
    }
}
