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
    public sealed class Projectile : IEntity<Guid>, IBody, IUpdatable, IDrawable
    {
        private readonly IPainter _draw;

        private readonly Sprite _sprite;

        private Vector2 _velocity;

        public Projectile(
            IPainter draw,
            Sprite sprite,
            float rotation,
            float speed)
        {
            _draw = draw;
            _sprite = sprite;
            _velocity = rotation.ToDirection() * speed;

            Id = Guid.NewGuid();
            Origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            Position = Vector2.Zero;
            Rotation = rotation;
            Scale = Vector2.One;
            Width = _sprite.Width;
            Height = _sprite.Height;
            Data = sprite.ReadData();
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

        void IUpdatable.Update(float time)
        {
            Position += _velocity * time;
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
