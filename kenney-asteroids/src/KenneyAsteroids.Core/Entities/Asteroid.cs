using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Asteroid : IEntity<Guid>, IUpdatable, Engine.IDrawable, IBody
    {
        private readonly IPainter _draw;

        private readonly Sprite _sprite;
        private readonly Vector2 _scale;
        private readonly float _rotationSpeed;

        private Vector2 _velocity;
        private float _rotation;

        public Asteroid(
            IPainter draw,
            Sprite sprite,
            Vector2 velocity,
            float rotationSpeed)
        {
            _sprite = sprite;
            _draw = draw;
            _velocity = velocity;
            _rotationSpeed = rotationSpeed;

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

        void IUpdatable.Update(float time)
        {
            Position += _velocity * time;
            _rotation += _rotationSpeed * time;
        }

        void Engine.IDrawable.Draw(float time)
        {
            _draw
                .Draw(
                    _sprite,
                    Position,
                    Origin,
                    _scale,
                    _rotation,
                    Color.White);
        }
    }
}
