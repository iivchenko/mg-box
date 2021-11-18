using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Ship : IEntity<Guid>, IUpdatable, IDrawable, IBody
    {
        private readonly IPainter _draw;
        private readonly Sprite _sprite;
        private readonly Weapon _weapon;
        private readonly float _maxSpeed;
        private readonly float _maxAcceleration;
        private readonly float _maxRotation;

        private Vector2 _velocity;
        private ShipAction _action;

        public Ship(
            IPainter draw,
            Sprite sprite,
            Weapon weapon,
            float maxSpeed,
            float maxAcceleration,
            float maxRotation)
        {
            _draw = draw;
            _sprite = sprite;
            _weapon = weapon;
            _maxSpeed = maxSpeed;
            _maxAcceleration = maxAcceleration;
            _maxRotation = maxRotation;

            _velocity = Vector2.Zero;
            _action = ShipAction.None;

            Id = Guid.NewGuid();
            Origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            Position = Vector2.Zero;
            Rotation = 0.0f;
            Scale = Vector2.One;
            Width = _sprite.Width;
            Height = _sprite.Height;
            Data = _sprite.ReadData();
        }
        
        public Guid Id { get; }
        public IEnumerable<string> Tags => Enumerable.Empty<string>();
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Color[] Data { get; set; }

        public void Apply(ShipAction action)
        {
            _action = action;
        }

        void IUpdatable.Update(float time)
        {
            _weapon.Update(time);

            if (_action.HasFlag(ShipAction.Left)) 
                Rotation -= _maxRotation * time;

            if (_action.HasFlag(ShipAction.Right))
                Rotation += _maxRotation * time;

            if (_action.HasFlag(ShipAction.Accelerate))
            {
                var velocity = _velocity + Rotation.ToDirection() * _maxAcceleration;

                _velocity = velocity.Length() > _maxSpeed ? Vector2.Normalize(velocity) * _maxSpeed : velocity;
            }

            Position += _velocity * time;

            if (_action.HasFlag(ShipAction.Fire))
                _weapon.Fire(Position, Rotation);

            _action = ShipAction.None;
        }

        void IDrawable.Draw(float time)
        {
            _draw
                .Draw(
                    _sprite,
                    Position,
                    Origin,
                    Scale,
                    Rotation,
                    Colors.White);
        }
    }

    [Flags]
    public enum ShipAction
    {
        None = 0b0000,
        Accelerate = 0b0001,
        Left = 0b0010,
        Right = 0b0100,
        Fire = 0b1000
    }
}
