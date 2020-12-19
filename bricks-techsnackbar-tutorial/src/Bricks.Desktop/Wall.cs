using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    public sealed class Wall
    {
        //We'll have 7 rows, each with its own color
        //there will be 10 bricks per row
        //there will be 3 blank rows at top
        //each brick is 50 x 16
        public Brick[,] BrickWall { get; set; }

        public Wall(float x, float y, SpriteBatch spriteBatch, GameContent gameContent)
        {
            BrickWall = new Brick[7, 10];
            float brickX = x;
            float brickY = y;
            Color color = Color.White;
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
                brickY = y + i * (gameContent.ImgBrick.Height);

                for (int j = 0; j < 10; j++)
                {
                    brickX = x + j * (gameContent.ImgBrick.Width);
                    Brick brick = new Brick(brickX, brickY, color, spriteBatch, gameContent);
                    BrickWall[i, j] = brick;
                }
            }
        }
        public void Draw()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    BrickWall[i, j].Draw();
                }
            }
        }
    }
}
