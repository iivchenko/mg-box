using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Ship : IEntity<Guid>, IUpdatable, Engine.IDrawable, IBody
    {
        private readonly Sprite _sprite;
        private readonly SpriteBatch _batch;
        private readonly Vector2 _scale;
        private readonly float _maxSpeed;
        private readonly float _acceleration;

        private Vector2 _velocity;
        private float _rotation;

        public Ship(
            Sprite sprite, 
            SpriteBatch batch,
            float maxSpeed,
            float acceleration)
        {
            _sprite = sprite;
            _batch = batch;
            _maxSpeed = maxSpeed;
            _acceleration = acceleration;

            _velocity = Vector2.Zero;
            _scale = Vector2.One;
            _rotation = 0.0f;

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

        void IUpdatable.Update(GameTime gameTime)
        {
            var direction = ReadDirection();
            var velocity = _velocity + direction * _acceleration;

            _velocity = velocity.Length() > _maxSpeed ? _velocity : velocity;
            _rotation = MathF.Atan2(_velocity.X, -_velocity.Y);

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
                    _rotation,
                    Color.White,
                    SpriteEffects.None);
        }

        private Vector2 ReadDirection()
        {
            // TODO: Move input logic out of the ship
            /* TODO: Redesign key controls:
             * A - accelerate
             * W - deccelerate
             * S - turn counter clock wise
             * D - turn clock wise
            */
            var keyboard = Keyboard.GetState();
            var x = 0;
            var y = 0;

            if (keyboard.IsKeyDown(Keys.W))
            {
                y += -1;
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                x += -1;
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                y += 1;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                x += 1;
            }

            return new Vector2(x, y);
        }
    }
}
