using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    public sealed class Paddle
    {
        private readonly Texture2D _sprite;
        private readonly SpriteBatch _spriteBatch;
        private readonly Vector2 _origin;
        private readonly float _screenWidth;

        private Vector2 _position;
        private Rectangle _body;

        public Paddle(Vector2 position, float screenWidth, SpriteBatch spriteBatch, Texture2D sprite)
        {
            _position = position;
            _screenWidth = screenWidth;
            _sprite = sprite;
            _spriteBatch = spriteBatch;

            _origin = Vector2.Zero;

            Width = _sprite.Width;
            Height = _sprite.Height;

            UpdateBody();
        }

        public Vector2 Position => _position;

        public Rectangle Body => _body;

        public float Width { get; set; }

        public float Height { get; set; }

        public void Draw()
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

            if (_position.X < 1)
            {
                _position.X = 1;
            }

            UpdateBody();
        }

        public void MoveRight()
        {
            _position.X += 5;

            if ((_position.X + Width) > _screenWidth)
            {
                _position.X = _screenWidth - Width;
            }

            UpdateBody();
        }

        public void MoveTo(float x)
        {
            if (x >= 0)
            {
                if (x < _screenWidth - Width)
                {
                    _position.X = x;
                }
                else
                {
                    _position.X = _screenWidth - Width;
                }
            }
            else
            {
                if (x < 0)
                {
                    _position.X = 0;
                }
            }

            UpdateBody();
        }

        private void UpdateBody()
        {
            _body = new Rectangle((int)_position.X, (int)_position.Y, (int)Width, (int)Height);
        }
    }
}
