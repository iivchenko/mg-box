using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using XMatrix = Microsoft.Xna.Framework.Matrix;
using XVector = Microsoft.Xna.Framework.Vector2;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Weapon : IEntity<Guid>, IUpdatable
    {
        private readonly Vector2 _offset;
        private readonly TimeSpan _reload;
        private readonly IProjectileFactory _factory;
        private readonly IEventPublisher _eventService;
        private readonly IAudioPlayer _player;

        private readonly Sound _lazer;

        private State _state;
        private double _reloading;

        public Weapon(
            Vector2 offset,
            TimeSpan reload,
            IProjectileFactory factory,
            IEventPublisher eventService,
            IAudioPlayer player,
            Sound lazer)
        {
            _offset = offset;
            _reload = reload;
            _factory = factory;
            _eventService = eventService;
            _player = player;

            _lazer = lazer;

            _state = State.Idle;

            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public IEnumerable<string> Tags => Enumerable.Empty<string>();

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
                var xnaOffset = new XVector(_offset.X, _offset.Y);
                var xnaParentPos = new XVector(parentPosition.X, parentPosition.Y);
                var xnaPosition = xnaOffset.Transform(XMatrix.CreateRotationZ(parentRotation)) + xnaParentPos;
                var position = new Vector2(xnaPosition.X, xnaPosition.Y);
                var direction = parentRotation.ToDirection();
                var projectile = _factory.Create(position, direction);

                _eventService.Publish(new EntityCreatedEvent(projectile));
                _player.Play(_lazer);
            }
        }

        private enum State
        {
            Idle,
            Reload
        }
    }
}
