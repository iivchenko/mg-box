using Bricks.Desktop.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Bricks.Desktop.Entities
{
    public static class WallFactory
    {
        public static List<Brick> CreateWall(float x, float y, Texture2D brick, SpriteBatch spriteBatch, IWorld world)
        {
            var wall = new Brick[7, 10];

            for (int i = 0; i < 7; i++)
            {
                var color = i switch
                {
                    0 => Color.Red,
                    1 => Color.Orange,
                    2 => Color.Yellow,
                    3 => Color.Green,
                    4 => Color.Blue,
                    5 => Color.Indigo,
                    6 => Color.Violet,
                    _ => Color.White
                };

                var brickY = y + i * brick.Height;

                for (int j = 0; j < 10; j++)
                {
                    var brickX = x + j * brick.Width;
                    wall[i, j] = new Brick(
                        new Vector2(brickX, brickY),
                        color,
                        spriteBatch,
                        brick,
                        7 - i,
                        world);
                }
            }

            return wall.OfType<Brick>().ToList();
        }
    }
}
