using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    public sealed class Paddle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float ScreenWidth { get; set; }

        private readonly Texture2D _sprite;
        private readonly SpriteBatch _spriteBatch;

        public Paddle(float x, float y, float screenWidth, SpriteBatch spriteBatch, Texture2D sprite)
        {
            X = x;
            Y = y;
            ScreenWidth = screenWidth;

            _sprite = sprite;
            _spriteBatch = spriteBatch;

            Width = _sprite.Width;
            Height = _sprite.Height;
        }

        public void Draw()
        {
            _spriteBatch.Draw(_sprite, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
        }

        public void MoveLeft()
        {
            X = X - 5;
            if (X < 1)
            {
                X = 1;
            }
        }
        public void MoveRight()
        {
            X = X + 5;
            if ((X + Width) > ScreenWidth)
            {
                X = ScreenWidth - Width;
            }
        }

        public void MoveTo(float x)
        {
            if (x >= 0)
            {
                if (x < ScreenWidth - Width)
                {
                    X = x;
                }
                else
                {
                    X = ScreenWidth - Width;
                }
            }
            else
            {
                if (x < 0)
                {
                    X = 0;
                }
            }
        }
    }
}
