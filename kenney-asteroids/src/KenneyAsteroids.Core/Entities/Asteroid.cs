using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using System;
using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Asteroid : IEntity<Guid>, IUpdatable, IDrawable, IBody
    {
        private readonly IPainter _draw;

        private readonly Sprite _sprite;
        private readonly float _rotationSpeed;

        private Vector2 _velocity;

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

            Id = Guid.NewGuid();
            Origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            Rotation = 0.0f;
            Position = Vector2.Zero;
            Width = _sprite.Width;
            Height = _sprite.Height;
            Scale = Vector2.One;
            Data = sprite.ReadData();
        }

        public Guid Id { get; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Color[] Data { get; set; }

        void IUpdatable.Update(float time)
        {
            Position += _velocity * time;
            Rotation += _rotationSpeed * time;
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
}
