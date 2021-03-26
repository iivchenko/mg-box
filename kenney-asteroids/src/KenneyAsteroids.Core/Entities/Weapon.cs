using KenneyAsteroids.Core.Screens.GamePlay; // TODO: recursive dependencie!!
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Eventing.Eventing;
using Microsoft.Xna.Framework;
using System;

using XVector = Microsoft.Xna.Framework.Vector2;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Weapon : IEntity<Guid>, IUpdatable
    {
        private readonly XVector _offset;
        private readonly TimeSpan _reload;
        private readonly IProjectileFactory _factory;
        private readonly IPublisher _eventService;

        private State _state;
        private double _reloading;

        public Weapon(
            XVector offset,
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

        public void Fire(XVector parentPosition, float parentRotation)
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
        Projectile Create(XVector position, XVector direction);
    }
}
