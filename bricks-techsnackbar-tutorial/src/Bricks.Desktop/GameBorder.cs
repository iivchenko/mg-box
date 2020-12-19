using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    class GameBorder
    {
        public float Width { get; set; } //width of game
        public float Height { get; set; } //height of game

        private Texture2D imgPixel { get; set; }  //cached image single pixel we'll use to draw the border lines
        private SpriteBatch spriteBatch;  //allows us to write on backbuffer when we need to draw self

        public GameBorder(float screenWidth, float screenHeight, SpriteBatch spriteBatch, GameContent gameContent)
        {
            Width = screenWidth;
            Height = screenHeight;
            imgPixel = gameContent.ImgPixel;
            this.spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            spriteBatch.Draw(imgPixel, new Rectangle(0, 0, (int)Width - 1, 1), Color.White);  //draw top border
            spriteBatch.Draw(imgPixel, new Rectangle(0, 0, 1, (int)Height - 1), Color.White);  //draw left border
            spriteBatch.Draw(imgPixel, new Rectangle((int)Width - 1, 0, 1, (int)Height - 1), Color.White);  //draw right border
        }
    }
}
