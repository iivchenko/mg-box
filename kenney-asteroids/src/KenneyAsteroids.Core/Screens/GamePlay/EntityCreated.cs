using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Eventing.Eventing;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class EntityCreatedEvent : GamePlayEvent
    {
        public EntityCreatedEvent(IEntity entity)
        {
            Entity = entity;
        }

        public IEntity Entity { get; }
    }

    public sealed class EntityCreatedEventHandler : IEventHandler<EntityCreatedEvent>
    {
        private readonly EntityCollection _entities;

        public EntityCreatedEventHandler(EntityCollection entities)
        {
            _entities = entities;
        }

        public void Handle(EntityCreatedEvent @event)
        {
            _entities.Add(@event.Entity);
        }
    }
}
