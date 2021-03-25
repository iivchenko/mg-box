using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Asteroid : IEntity<Guid>, IUpdatable, Engine.IDrawable, IBody
    {
        private readonly IDrawSystem _draw;

        private readonly Sprite _sprite;
        private readonly Vector _scale;
        private readonly float _rotationSpeed;

        private Vector _velocity;
        private float _rotation;

        public Asteroid(
            IDrawSystem draw,
            Sprite sprite,
            Vector velocity,
            float rotationSpeed)
        {
            _sprite = sprite;
            _draw = draw;
            _velocity = velocity;
            _rotationSpeed = rotationSpeed;

            _scale = Vector.One;
            _rotation = 0.0f;

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
