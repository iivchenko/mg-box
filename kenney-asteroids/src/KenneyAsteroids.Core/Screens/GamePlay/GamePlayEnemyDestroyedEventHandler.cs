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

        public bool ExecuteCondition(EntityDestroyedEvent @event) => @event.Entity is Asteroid;

        public void ExecuteAction(EntityDestroyedEvent @event)
        {
            _publisher.Publish(new GamePlayCreateAsteroidCommand());
        }
    }
}
