using KenneyAsteroids.Engine.Messaging;
using System;

namespace KenneyAsteroids.Engine.Particles
{
    public sealed class ParticlesEmmisionFinishedEvent : IMessage
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
