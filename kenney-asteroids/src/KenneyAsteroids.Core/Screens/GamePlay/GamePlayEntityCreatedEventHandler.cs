using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Rules;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayEntityCreatedEventHandler : IRule<EntityCreatedEvent>
    {
        private readonly IEntitySystem _entities;

        public GamePlayEntityCreatedEventHandler(IEntitySystem entities)
        {
            _entities = entities;
        }

        public bool ExecuteCondition(EntityCreatedEvent @event) => true;

        public void ExecuteAction(EntityCreatedEvent @event)
        {
            _entities.Add(@event.Entity);
        }
    }
}
