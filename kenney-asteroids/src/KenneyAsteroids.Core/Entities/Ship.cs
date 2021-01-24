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
            float maxSpeed,
            float maxAcceleration,
            float maxRotation)
        {
            _sprite = sprite;
            _batch = batch;
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

        void IUpdatable.Update(GameTime gameTime)
        {
            if (_action.HasFlag(ShipAction.Left))
            {
                _rotation -= _maxRotation * gameTime.ToDelta();
            }

            if (_action.HasFlag(ShipAction.Right))
            {
                _rotation += _maxRotation * gameTime.ToDelta();
            }

            if (_action.HasFlag(ShipAction.Accelerate))
            {
                var direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));
                var velocity = _velocity + direction * _maxAcceleration;

                _velocity = velocity.Length() > _maxSpeed ? _velocity : velocity;
            }

            Position += _velocity * gameTime.ToDelta();
        }

        void Engine.IDrawable.Draw(GameTime gameTime)
        {
            _batch
                .Draw(
                    _sprite,
                    Position,
                    Origin,
                    _scale,
                    _rotation + MathHelper.ToRadians(90), // TODO: Hack! move to the sprite additional rotation
                    Color.White,
                    SpriteEffects.None);
        }
    }

    [Flags]
    public enum ShipAction
    {
        None = 0,
        Accelerate = 1,
        Left = 2,
        Right = 4
    }
}
