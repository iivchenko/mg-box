using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;

using System;

using XVector = Microsoft.Xna.Framework.Vector2;
using XColor = Microsoft.Xna.Framework.Color;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Asteroid : IEntity<Guid>, IUpdatable, IDrawable, IBody
    {
        private readonly IDrawSystem _draw;

        private readonly Sprite _sprite;
        private readonly XVector _scale;
        private readonly float _rotationSpeed;

        private XVector _velocity;
        private float _rotation;

        public Asteroid(
            IDrawSystem draw,
            Sprite sprite,
            XVector velocity,
            float rotationSpeed)
        {
            _sprite = sprite;
            _draw = draw;
            _velocity = velocity;
            _rotationSpeed = rotationSpeed;

            _scale = XVector.One;
            _rotation = 0.0f;

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
            _rotation += _rotationSpeed * time;
        }

        void IDrawable.Draw(float time)
        {
            _draw
                .Draw(
                    _sprite,
                    Position,
                    Origin,
                    _scale,
                    _rotation,
                    XColor.White);
        }
    }
}
