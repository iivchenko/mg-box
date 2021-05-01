using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;
using System;

namespace KenneyAsteroids.Core.Events
{
    public sealed class EntityDestroyedEvent : IMessage
    {
        public EntityDestroyedEvent(IEntity entity)
        {
            Id = Guid.NewGuid();

            Entity = entity;
        }

        public Guid Id { get; }

        public IEntity Entity { get; }
    }
}
