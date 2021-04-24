using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;

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

    public sealed class EntityCreatedEventHandler : IMessageHandler<EntityCreatedEvent>
    {
        private readonly IEntitySystem _entities;

        public EntityCreatedEventHandler(IEntitySystem entities)
        {
            _entities = entities;
        }

        public void Handle(EntityCreatedEvent @event)
        {
            _entities.Add(@event.Entity);
        }
    }
}
