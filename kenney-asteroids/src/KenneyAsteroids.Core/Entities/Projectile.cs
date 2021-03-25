using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Projectile : IEntity<Guid>, IBody, IUpdatable, Engine.IDrawable
    {
        private readonly IDrawSystem _draw;

        private readonly Sprite _sprite;
        private readonly float _rotation;

        private Vector _velocity;

        public Projectile(
            IDrawSystem draw,
            Sprite sprite,
            float rotation,
            float speed)
        {
            _draw = draw;
            _sprite = sprite;
            _rotation = rotation;
            _velocity = rotation.ToDirection() * speed;

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
        }

        void IDrawable.Draw(float time)
        {
            _draw
                .Draw(
                    _sprite,
                    Position,
                    Origin,
                    Vector.One,
                    _rotation,
                    Color.White);
        }
    }
}
