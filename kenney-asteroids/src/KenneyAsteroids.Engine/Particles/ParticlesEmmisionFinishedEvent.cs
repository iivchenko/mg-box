using KenneyAsteroids.Engine.Rules;
using System;

namespace KenneyAsteroids.Engine.Particles
{
    public sealed class ParticlesEmmisionFinishedEvent : IEvent
    {
        public ParticlesEmmisionFinishedEvent(ParticleEngine engine)
        {
            Id = Guid.NewGuid();

            Engine = engine;
        }

        public Guid Id { get; }

        public ParticleEngine Engine { get; }
    }
}
