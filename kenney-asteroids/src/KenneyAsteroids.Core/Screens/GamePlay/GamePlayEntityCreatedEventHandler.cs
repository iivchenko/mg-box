using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayEntityCreatedEventHandler : IMessageHandler<EntityCreatedEvent>
    {
        private readonly IEntitySystem _entities;

        public GamePlayEntityCreatedEventHandler(IEntitySystem entities)
        {
            _entities = entities;
        }

        public void Handle(EntityCreatedEvent @event)
        {
            _entities.Add(@event.Entity);
        }
    }
}
