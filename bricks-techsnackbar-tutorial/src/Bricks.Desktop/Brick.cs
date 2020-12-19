using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    public sealed class Brick
    {
        public float X { get; set; } //x position of brick on screen
        public float Y { get; set; } //y position of brick on screen
        public float Width { get; set; } //width of brick
        public float Height { get; set; } //height of brick
        public bool Visible { get; set; } //does brick still exist?
        private Color _color;

        private readonly Texture2D _imgBrick;
        private readonly SpriteBatch _spriteBatch;

        public Brick(float x, float y, Color color, SpriteBatch spriteBatch, GameContent gameContent)
        {
            X = x;
            Y = y;

            _imgBrick = gameContent.ImgBrick;
            _color = color;
            _spriteBatch = spriteBatch;

            Width = _imgBrick.Width;
            Height = _imgBrick.Height;
            Visible = true;
        }

        public void Draw()
        {
            if (Visible)
            {
                _spriteBatch.Draw(_imgBrick, new Vector2(X, Y), null, _color, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
