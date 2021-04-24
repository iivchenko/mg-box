using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Eventing.Eventing;
using KenneyAsteroids.Engine.Graphics;
using System;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class EnemySpawner : IUpdatable
    {
        private readonly Random _random;
        private readonly IUpdatable _timer;
        private readonly IViewport _viewport;
        private readonly EntityFactory _factory;
        private readonly IPublisher _eventService;

        public EnemySpawner(IViewport viewport, EntityFactory factory, IPublisher eventService)
        {
            _viewport = viewport;
            _factory = factory;
            _eventService = eventService;

            _random = new Random();
            var timer = new Timer(TimeSpan.FromSeconds(5), SpawnAsteroid, true);
            _timer = timer;

            timer.Start();
        }

        public void Update(float time)
        {
            _timer.Update(time);
        }

        private void SpawnAsteroid(float time)
        {
            const int BigAsteroidMinSpeed = 15;
            const int BigAsteroidMaxSpeed = 100;
            const int BigAsteroidMinRotationSpeed = 5;
            const int BigAsteroidMaxRotationSpeed = 25;

            var x = 0;
            var y = 0;
            var dx = 0;
            var dy = 0;

            switch (_random.Next(0, 4))
            {
                case 0: // Up -> Down
                    x = _random.Next(0, (int)_viewport.Width);
                    y = 0;
                    dx = _random.Next(0, (int)_viewport.Width);
                    dy = (int)_viewport.Height;
                    break;

                case 1: // Right -> Left
                    x = (int)_viewport.Width;
                    y = _random.Next(0, (int)_viewport.Height);
                    dx = 0;
                    dy = _random.Next(0, (int)_viewport.Height);
                    break;

                case 2: // Down -> UP
                    x = _random.Next(0, (int)_viewport.Width);
                    y = (int)_viewport.Height;
                    dx = _random.Next(0, (int)_viewport.Width);
                    dy = 0;
                    break;

                case 3: // Left -> Right
                    x = 0;
                    y = _random.Next(0, (int)_viewport.Height);
                    dx = (int)_viewport.Width;
                    dy = _random.Next(0, (int)_viewport.Height);
                    break;
            }

            var position = new Vector2(x, y);
            var direction = Vector2.Normalize(new Vector2(dx - x, dy - y));

            var velocity = direction * new Vector2(_random.Next(BigAsteroidMinSpeed, BigAsteroidMaxSpeed), _random.Next(BigAsteroidMinSpeed, BigAsteroidMaxSpeed));
            var rotationSpeed = _random.Next(BigAsteroidMinRotationSpeed, BigAsteroidMaxRotationSpeed).AsRadians() * _random.NextDouble() > 0.5 ? 1 : -1;

            var asteroid = _factory.CreateAsteroid(position, velocity, rotationSpeed);

            _eventService.Publish(new EntityCreatedEvent(asteroid));
        }
    }
}
