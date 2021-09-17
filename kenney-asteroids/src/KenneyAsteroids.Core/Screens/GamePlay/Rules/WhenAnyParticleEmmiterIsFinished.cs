using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;
using KenneyAsteroids.Engine.Particles;

namespace KenneyAsteroids.Core.Screens.GamePlay.Rules
{
    public static class WhenAnyParticleEmmiterIsFinished
    {
        public sealed class ThenRemoveEmmiterFromEntities : IMessageHandler<ParticlesEmmisionFinishedEvent>
        {
            private readonly IEntitySystem _entities;

            public ThenRemoveEmmiterFromEntities(
               IEntitySystem entities)
            {
                _entities = entities;
            }

            public void Execute(ParticlesEmmisionFinishedEvent message)
            {
                _entities.Remove(message.Engine);
            }
        }
    }
}
