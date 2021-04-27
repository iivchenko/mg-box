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
}
