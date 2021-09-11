using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;
using KenneyAsteroids.Engine.Particles;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class WhenAnyParticleEngineFinished_RemoveItFromEntities :
               IMessageHandler<ParticlesEmmisionFinishedEvent>
    {
        private readonly IEntitySystem _entities;

        public WhenAnyParticleEngineFinished_RemoveItFromEntities(
           IEntitySystem entities)
        {
            _entities = entities;
        }

        public void Handle(ParticlesEmmisionFinishedEvent message)
        {
            _entities.Remove(message.Engine);
        }
    }
}
