using Bricks.Desktop.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop.Entities
{
    public sealed class Paddle : IEntity
    {
        private readonly Texture2D _sprite;
        private readonly SpriteBatch _spriteBatch;
        private readonly Vector2 _origin;

        private Vector2 _position;

        public Paddle(Vector2 position, SpriteBatch spriteBatch, Texture2D sprite)
        {
            _position = position;
            _sprite = sprite;
            _spriteBatch = spriteBatch;

            _origin = Vector2.Zero;

            Width = _sprite.Width;
            Height = _sprite.Height;
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Rectangle Body => new Rectangle((int)_position.X, (int)_position.Y, (int)Width, (int)Height);

        public float Width { get; }

        public float Height { get; }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(
               _sprite,
               _position,
               null,
               Color.White,
               0,
               _origin,
               1.0f,
               SpriteEffects.None,
               0);
        }

        public void MoveLeft()
        {
            _position.X -= 5;
        }

        public void MoveRight()
        {
            _position.X += 5;
        }

        public void MoveTo(float x)
        {
            _position.X = x;
        }       
    }
}
