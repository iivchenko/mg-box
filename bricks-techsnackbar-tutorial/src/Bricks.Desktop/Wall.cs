using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    public sealed class Wall
    {
        private readonly Texture2D _brick;

        //We'll have 7 rows, each with its own color
        //there will be 10 bricks per row
        //there will be 3 blank rows at top
        //each brick is 50 x 16
        public Brick[,] BrickWall { get; set; }

        public Wall(float x, float y, SpriteBatch spriteBatch, Texture2D brick)
        {
            _brick = brick;

            BrickWall = new Brick[7, 10];
            var color = Color.White;
            for (int i = 0; i < 7; i++)
            {
                switch (i)
                {
                    case 0:
                        color = Color.Red;
                        break;
                    case 1:
                        color = Color.Orange;
                        break;
                    case 2:
                        color = Color.Yellow;
                        break;
                    case 3:
                        color = Color.Green;
                        break;
                    case 4:
                        color = Color.Blue;
                        break;
                    case 5:
                        color = Color.Indigo;
                        break;
                    case 6:
                        color = Color.Violet;
                        break;
                }
                var brickY = y + i * _brick.Height;

                for (int j = 0; j < 10; j++)
                {
                    var brickX = x + j * _brick.Width;
                    BrickWall[i, j] = new Brick(new Vector2(brickX, brickY), color, spriteBatch, _brick);
                }
            }
        }

        public void Draw()
        {
            foreach(var brick in BrickWall)
            {
                brick.Draw();
            }
        }
    }
}
