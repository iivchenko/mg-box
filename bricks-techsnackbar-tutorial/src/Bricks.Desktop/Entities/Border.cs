using Bricks.Desktop.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop.Entities
{
    public sealed class Border : IEntity
    {
        private readonly Texture2D _pixel;

        public float Width { get; set; }

        public float Height { get; set; }

        public Vector2 Position => throw new System.NotImplementedException();

        private SpriteBatch _spriteBatch;

        public Border(float screenWidth, float screenHeight, SpriteBatch spriteBatch, Texture2D pixel)
        {
            Width = screenWidth;
            Height = screenHeight;

            _spriteBatch = spriteBatch;
            _pixel = pixel;
        }

        public void Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, (int)Width - 1, 1), Color.White);
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, 1, (int)Height - 1), Color.White);
            _spriteBatch.Draw(_pixel, new Rectangle((int)Width - 1, 0, 1, (int)Height - 1), Color.White);
        }
    }
}
