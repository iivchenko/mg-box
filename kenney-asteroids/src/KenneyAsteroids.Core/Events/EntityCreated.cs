using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;
using System;

namespace KenneyAsteroids.Core.Events
{
    public sealed class EntityCreatedEvent : IMessage
    {
        public EntityCreatedEvent(IEntity entity)
        {
            Entity = entity;

            Id = Guid.NewGuid();
        }

        public IEntity Entity { get; }

        public Guid Id { get; }
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
