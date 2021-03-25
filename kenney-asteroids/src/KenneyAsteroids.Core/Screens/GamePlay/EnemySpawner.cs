using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Eventing.Eventing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class EnemySpawner : IUpdatable
    {
        private readonly Random _random;
        private readonly IUpdatable _timer;
        private readonly Viewport _viewport;
        private readonly EntityFactory _factory;
        private readonly IPublisher _eventService;

        public EnemySpawner(Viewport viewport, EntityFactory factory, IPublisher eventService)
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
                    x = _random.Next(0, _viewport.Width);
                    y = 0;
                    dx = _random.Next(0, _viewport.Width);
                    dy = _viewport.Height;
                    break;

                case 1: // Right -> Left
                    x = _viewport.Width;
                    y = _random.Next(0, _viewport.Height);
                    dx = 0;
                    dy = _random.Next(0, _viewport.Height);
                    break;

                case 2: // Down -> UP
                    x = _random.Next(0, _viewport.Width);
                    y = _viewport.Height;
                    dx = _random.Next(0, _viewport.Width);
                    dy = 0;
                    break;

                case 3: // Left -> Right
                    x = 0;
                    y = _random.Next(0, _viewport.Height);
                    dx = _viewport.Width;
                    dy = _random.Next(0, _viewport.Height);
                    break;
            }

            var position = new Vector(x, y);
            var direction = new Vector(dx, dy) - position;
            var velocity = direction.Normalize() * new Vector(_random.Next(BigAsteroidMinSpeed, BigAsteroidMaxSpeed), _random.Next(BigAsteroidMinSpeed, BigAsteroidMaxSpeed));
            var rotationSpeed = _random.Next(BigAsteroidMinRotationSpeed, BigAsteroidMaxRotationSpeed).AsRadians() * _random.NextDouble() > 0.5 ? 1 : -1;

            var asteroid = _factory.CreateAsteroid(position, velocity, rotationSpeed);

            _eventService.Publish(new EntityCreatedEvent(asteroid));
        }
    }
}
