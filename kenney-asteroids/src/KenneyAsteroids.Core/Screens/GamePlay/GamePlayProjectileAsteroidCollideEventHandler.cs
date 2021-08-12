using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Messaging;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayProjectileAsteroidCollideEventHandler :
       IMessageHandler<GamePlayEntitiesCollideEvent<Projectile, Asteroid>>
    {
        private readonly GamePlayHud _hud;
        private readonly IEntitySystem _entities;
        private readonly IPublisher _publisher;
        private readonly GamePlayScoreManager _scores;

        public GamePlayProjectileAsteroidCollideEventHandler(
           GamePlayHud hud,
           IEntitySystem entities,
           IPublisher publisher)
        {
            _hud = hud;
            _entities = entities;
            _publisher = publisher;

            _scores = new GamePlayScoreManager();
        }

        public void Handle(GamePlayEntitiesCollideEvent<Projectile, Asteroid> message)
        {
            var projectile = message.Body1;
            var asteroid = message.Body2;

            _entities.Remove(projectile, asteroid);
            _hud.Scores += _scores.GetScore(asteroid);
            _publisher.Publish(new EntityDestroyedEvent(asteroid));
        }
    }
}
