using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop
{
    public sealed class Brick
    {
        private readonly Vector2 _rotation = new Vector2(0, 0);
        private readonly Texture2D _sprite;
        private readonly Color _color;

        private readonly SpriteBatch _spriteBatch;

        private Vector2 _position;

        public Brick(
            Vector2 position, 
            Color color, 
            SpriteBatch spriteBatch, 
            Texture2D sprite)
        {
            _position = position;

            _sprite = sprite;
            _color = color;

            _spriteBatch = spriteBatch;

            Width = _sprite.Width;
            Height = _sprite.Height;
            Visible = true;
        }

        public Vector2 Position => _position;

        public float Width { get; }

        public float Height { get; }

        public bool Visible { get; set; }

        public void Draw()
        {
            if (Visible)
            {
                _spriteBatch.Draw(
                    _sprite,
                    _position,
                    null,
                    _color,
                    0,
                    _rotation,
                    1.0f,
                    SpriteEffects.None,
                    0);
            }
        }
    }
}
