using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine.Rules;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayEnemyDestroyedEventHandler : IRule<EntityDestroyedEvent>
    {
        public IEventPublisher _publisher;

        public GamePlayEnemyDestroyedEventHandler(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Execute(EntityDestroyedEvent @event)
        {
            if (@event.Entity is Asteroid)
            {
                _publisher.Publish(new GamePlayCreateAsteroidCommand());
            }
        }
    }
}
