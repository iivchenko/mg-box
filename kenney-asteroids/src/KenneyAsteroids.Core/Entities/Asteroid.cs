using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Asteroid : Entity, IUpdatable, Engine.IDrawable
    {
        private readonly Sprite _sprite;
        private readonly SpriteBatch _batch;
        private readonly Vector2 _scale;
        private readonly float _rotationSpeed;

        private Vector2 _velocity;
        private float _rotation;

        public Asteroid(
            Sprite sprite,
            SpriteBatch batch,
            Vector2 velocity,
            float rotationSpeed)
        {
            _sprite = sprite;
            _batch = batch;
            _velocity = velocity;
            _rotationSpeed = rotationSpeed;

            _scale = Vector2.One;
            _rotation = 0.0f;

            Origin = new Vector2(_sprite.Width / 2.0f, _sprite.Height / 2.0f);
            Position = Vector2.Zero;
            Width = _sprite.Width;
            Height = _sprite.Height;
        }

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
                    Position,
                    Origin,
                    _scale,
                    _rotation,
                    Color.White,
                    SpriteEffects.None);
        }
    }
}
