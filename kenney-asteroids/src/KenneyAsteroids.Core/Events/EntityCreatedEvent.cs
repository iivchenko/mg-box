using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Rules;
using System;

namespace KenneyAsteroids.Core.Events
{
    public sealed class EntityCreatedEvent : IEvent
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
