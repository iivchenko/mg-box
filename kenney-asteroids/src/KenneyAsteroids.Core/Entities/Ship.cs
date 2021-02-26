using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Ship : IEntity<Guid>, IUpdatable, Engine.IDrawable, IBody
    {
        private readonly Sprite _sprite;
        private readonly SpriteBatch _batch;
        private readonly Weapon _weapon;
        private readonly Vector2 _scale;
        private readonly float _maxSpeed;
        private readonly float _maxAcceleration;
        private readonly float _maxRotation;

        private Vector2 _velocity;
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

            _velocity = Vector2.Zero;
            _scale = Vector2.One;
            _rotation = 0.0f;
            _action = ShipAction.None;

            Id = Guid.NewGuid();
            Origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            Position = Vector2.Zero;
            Width = _sprite.Width;
            Height = _sprite.Height;
        }
        
        public Guid Id { get; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public void Apply(ShipAction action)
        {
            _action = action;
        }

        void IUpdatable.Update(GameTime time)
        {
            _weapon.Update(time);

            if (_action.HasFlag(ShipAction.Left)) 
                _rotation -= _maxRotation * time.ToDelta();

            if (_action.HasFlag(ShipAction.Right))
                _rotation += _maxRotation * time.ToDelta();

            if (_action.HasFlag(ShipAction.Accelerate))
            {
                var velocity = _velocity + _rotation.ToDirection() * _maxAcceleration;

                _velocity = velocity.Length() > _maxSpeed ? _velocity : velocity;
            }

            Position += _velocity * time.ToDelta();

            if (_action.HasFlag(ShipAction.Fire))
                _weapon.Fire(Position, _rotation);

            _action = ShipAction.None;
        }

        void Engine.IDrawable.Draw(GameTime time)
        {
            _batch
                .Draw(
                    _sprite,
                    Position,
                    Origin,
                    _scale,
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
