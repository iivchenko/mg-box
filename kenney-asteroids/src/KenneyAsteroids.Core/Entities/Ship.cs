using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Ship : IEntity<Guid>, IUpdatable, Engine.IDrawable, IBody
    {
        private readonly Sprite _sprite;
        private readonly SpriteBatch _batch;
        private readonly Weapon _weapon;
        private readonly Vector _scale;
        private readonly float _maxSpeed;
        private readonly float _maxAcceleration;
        private readonly float _maxRotation;

        private Vector _velocity;
        private float _rotation;
        private ShipAction _action;

        public Ship(
            Sprite sprite, 
            SpriteBatch batch,
            Weapon weapon,
            float maxSpeed,
            float maxAcceleration,
            float maxRotation)
        {
            _sprite = sprite;
            _batch = batch;
            _weapon = weapon;
            _maxSpeed = maxSpeed;
            _maxAcceleration = maxAcceleration;
            _maxRotation = maxRotation;

            _velocity = Vector.Zero;
            _scale = Vector.One;
            _rotation = 0.0f;
            _action = ShipAction.None;

            Id = Guid.NewGuid();
            Origin = new Vector(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            Position = Vector.Zero;
            Width = _sprite.Width;
            Height = _sprite.Height;
        }
        
        public Guid Id { get; }
        public Vector Position { get; set; }
        public Vector Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public void Apply(ShipAction action)
        {
            _action = action;
        }

        void IUpdatable.Update(float time)
        {
            _weapon.Update(time);

            if (_action.HasFlag(ShipAction.Left)) 
                _rotation -= _maxRotation * time;

            if (_action.HasFlag(ShipAction.Right))
                _rotation += _maxRotation * time;

            if (_action.HasFlag(ShipAction.Accelerate))
            {
                var velocity = _velocity + _rotation.ToDirection() * _maxAcceleration;

                _velocity = velocity.Length() > _maxSpeed ? velocity.Normalize() * _maxSpeed : velocity;
            }

            Position += _velocity * time;

            if (_action.HasFlag(ShipAction.Fire))
                _weapon.Fire(Position, _rotation);

            _action = ShipAction.None;
        }

        void Engine.IDrawable.Draw(float time)
        {
            _batch
                .Draw(
                    _sprite,
                    Position.ToXna(),
                    Origin.ToXna(),
                    _scale.ToXna(),
                    _rotation,
                    Color.White,
                    SpriteEffects.None);
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
