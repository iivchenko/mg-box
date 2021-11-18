using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Rules;
using KenneyAsteroids.Engine.Particles;

namespace KenneyAsteroids.Core.Screens.GamePlay.Rules
{
    public static class PartileRules
    {
        public static class WhenAnyParticleEmmiterIsFinished
        {
            public sealed class ThenRemoveEmmiterFromEntities : IRule<ParticlesEmmisionFinishedEvent>
            {
                private readonly IEntitySystem _entities;

                public ThenRemoveEmmiterFromEntities(
                   IEntitySystem entities)
                {
                    _entities = entities;
                }

                public bool ExecuteCondition(ParticlesEmmisionFinishedEvent @event) => true;

                public void ExecuteAction(ParticlesEmmisionFinishedEvent @event)
                {
                    _entities.Remove(@event.Engine);
                }
            }
        }
    }
}
