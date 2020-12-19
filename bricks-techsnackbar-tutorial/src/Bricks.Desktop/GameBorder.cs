using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    public sealed class GameBorder
    {
        private readonly Texture2D _pixel;
        public float Width { get; set; } //width of game
        public float Height { get; set; } //height of game

        private SpriteBatch _spriteBatch;  //allows us to write on backbuffer when we need to draw self

        public GameBorder(float screenWidth, float screenHeight, SpriteBatch spriteBatch, Texture2D pixel)
        {
            Width = screenWidth;
            Height = screenHeight;

            _spriteBatch = spriteBatch;
            _pixel = pixel;
        }

        public void Draw()
        {
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, (int)Width - 1, 1), Color.White);  //draw top border
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, 1, (int)Height - 1), Color.White);  //draw left border
            _spriteBatch.Draw(_pixel, new Rectangle((int)Width - 1, 0, 1, (int)Height - 1), Color.White);  //draw right border
        }
    }
}
