using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Rules;
using System;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayCreateAsteroidCommand : IEvent
    {
        public GamePlayCreateAsteroidCommand()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }

    public sealed class GamePlayCreateAsteroidCommandHandler : IRule<GamePlayCreateAsteroidCommand>
    {
        private readonly Random _random;
        private readonly IViewport _viewport;
        private readonly IEntityFactory _factory;
        private readonly IEventPublisher _publisher;

        public GamePlayCreateAsteroidCommandHandler(IViewport viewport, IEntityFactory factory, IEventPublisher eventService)
        {
            _viewport = viewport;
            _factory = factory;
            _publisher = eventService;

            _random = new Random();
        }

        public void Execute(GamePlayCreateAsteroidCommand @event)
        {
            var x = 0;
            var y = 0;

            switch (_random.Next(0, 4))
            {
                case 0: // Up -> Down
                    x = _random.Next(0, (int)_viewport.Width);
                    y = 0;
                    break;

                case 1: // Right -> Left
                    x = (int)_viewport.Width;
                    y = _random.Next(0, (int)_viewport.Height);
                    break;

                case 2: // Down -> UP
                    x = _random.Next(0, (int)_viewport.Width);
                    y = (int)_viewport.Height;
                    break;

                case 3: // Left -> Right
                    x = 0;
                    y = _random.Next(0, (int)_viewport.Height);
                    break;
            }

            var position = new Vector2(x, y);
            var direction = _random.Next(0, 360).AsRadians();
            var type = new[] { AsteroidType.Tiny, AsteroidType.Small, AsteroidType.Medium, AsteroidType.Big }.RandomPick();
            var asteroid = _factory.CreateAsteroid(type, position, direction);

            _publisher.Publish(new EntityCreatedEvent(asteroid));
        }
    }
}
