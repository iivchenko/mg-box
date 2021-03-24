using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Asteroid : IEntity<Guid>, IUpdatable, Engine.IDrawable, IBody
    {
        private readonly Sprite _sprite;
        private readonly SpriteBatch _batch;
        private readonly Vector _scale;
        private readonly float _rotationSpeed;

        private Vector _velocity;
        private float _rotation;

        public Asteroid(
            Sprite sprite,
            SpriteBatch batch,
            Vector velocity,
            float rotationSpeed)
        {
            _sprite = sprite;
            _batch = batch;
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

        void IUpdatable.Update(GameTime gameTime)
        {
            Position += _velocity * gameTime.ToDelta();
            _rotation += _rotationSpeed * gameTime.ToDelta();
        }

        void Engine.IDrawable.Draw(GameTime gameTime)
        {
            _batch
                .Draw(
                    _sprite,
                    Position.ToXna(),
                    Origin.ToXna(),
                    _scale.ToXna(),
                    _rotation,
                    Color.White,
                    SpriteEffects.None);
        }
    }
}
