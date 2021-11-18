using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using System;
using System.Collections.Generic;
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
            AsteroidType type,
            Sprite sprite,
            Vector2 velocity,
            Vector2 scale,
            float rotationSpeed)
        {
            _sprite = sprite;
            _draw = draw;
            _velocity = velocity;
            _rotationSpeed = rotationSpeed;

            Id = Guid.NewGuid();
            Type = type;
            Origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            Rotation = 0.0f;
            Position = Vector2.Zero;
            Scale = scale;
            Width = _sprite.Width;
            Height = _sprite.Height;
            Data = sprite.ReadData();
        }

        public Guid Id { get; }
        public IEnumerable<string> Tags => new[] { GameTags.Enemy };
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Color[] Data { get; set; }
        public AsteroidType Type { get; set; }
        public Vector2 Velocity => _velocity;

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

    public enum AsteroidType
    {
        Tiny, 
        Small, 
        Medium, 
        Big
    }
}
