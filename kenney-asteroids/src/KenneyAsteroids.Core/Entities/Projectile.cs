using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Projectile : IEntity<Guid>, IBody, IUpdatable, Engine.IDrawable
    {
        private readonly Sprite _sprite;
        private readonly SpriteBatch _batch;
        private readonly float _rotation;

        private Vector2 _velocity;

        public Projectile(
            Sprite sprite,
            SpriteBatch batch,
            float rotation,
            float speed)
        {
            _sprite = sprite;
            _batch = batch;
            _rotation = rotation;
            _velocity = rotation.ToDirection() * speed;

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

        void IUpdatable.Update(GameTime time)
        {
            Position += _velocity * time.ToDelta();
        }

        void Engine.IDrawable.Draw(GameTime time)
        {
            _batch
                .Draw(
                    _sprite,
                    Position,
                    Origin,
                    Vector2.One,
                    _rotation,
                    Color.White,
                    SpriteEffects.None);
        }
    }
}
