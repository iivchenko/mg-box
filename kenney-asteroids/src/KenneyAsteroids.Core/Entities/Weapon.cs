using KenneyAsteroids.Core.Screens.GamePlay; // TODO: recursive dependencie!!
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Eventing.Eventing;
using Microsoft.Xna.Framework;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Weapon : IEntity<Guid>, IUpdatable
    {
        private readonly Vector2 _offset;
        private readonly TimeSpan _reload;
        private readonly IProjectileFactory _factory;
        private readonly IPublisher _eventService;

        private State _state;
        private double _reloading;

        public Weapon(
            Vector2 offset,
            TimeSpan reload,
            IProjectileFactory factory,
            IPublisher eventService)
        {
            _offset = offset;
            _reload = reload;
            _factory = factory;
            _eventService = eventService;

            _state = State.Idle;

            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public void Update(float time)
        {
            switch(_state)
            {
                case State.Idle:
                    break;
                case State.Reload:
                    _reloading -= time;

                    if (_reloading <= 0)
                    {
                        _state = State.Idle;
                    }
                    break;
            }
        }

        public void Fire(Vector2 parentPosition, float parentRotation)
        {
            if (_state == State.Idle)
            {
                _state = State.Reload;
                _reloading = _reload.TotalSeconds;
                var position = _offset.Transform(Matrix.CreateRotationZ(parentRotation)) + parentPosition;
                var direction = parentRotation.ToDirection();
                var projectile = _factory.Create(position, direction);

                _eventService.Publish(new EntityCreatedEvent(projectile));
            }
        }

        private enum State
        {
            Idle,
            Reload
        }
    }

    public interface IProjectileFactory
    {
        Projectile Create(Vector2 position, Vector2 direction);
    }
}
