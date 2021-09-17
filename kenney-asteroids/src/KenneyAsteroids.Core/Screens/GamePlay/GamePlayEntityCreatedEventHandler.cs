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

        public void Execute(EntityCreatedEvent @event)
        {
            _entities.Add(@event.Entity);
        }
    }
}
