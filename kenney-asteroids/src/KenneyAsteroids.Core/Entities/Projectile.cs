using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;

using System;

using XVector = Microsoft.Xna.Framework.Vector2;
using XColor = Microsoft.Xna.Framework.Color;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Projectile : IEntity<Guid>, IBody, IUpdatable, Engine.IDrawable
    {
        private readonly IDrawSystem _draw;

        private readonly Sprite _sprite;
        private readonly float _rotation;

        private XVector _velocity;

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
            Origin = new XVector(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            Position = XVector.Zero;
            Width = _sprite.Width;
            Height = _sprite.Height;
        }

        public Guid Id { get; }
        public XVector Position { get; set; }
        public XVector Origin { get; set; }
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
                    XVector.One,
                    _rotation,
                    XColor.White);
        }
    }
}
