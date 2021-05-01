using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine.Messaging;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayEnemyDestroyedEventHandler : IMessageHandler<EntityDestroyedEvent>
    {
        public IPublisher _publisher;

        public GamePlayEnemyDestroyedEventHandler(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(EntityDestroyedEvent @event)
        {
            if (@event.Entity is Asteroid)
            {
                _publisher.Publish(new CreateAsteroidCommand());
            }
        }
    }
}
