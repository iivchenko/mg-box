using KenneyAsteroids.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Core.Entities
{
    // TODO: Move from game play to manin menu
    public sealed class Version : IEntity, Engine.IDrawable
    {
        public readonly string V = "0.1";

        private readonly Vector2 _position;
        private readonly SpriteFont _font;
        private readonly Viewport _viewport;
        private readonly SpriteBatch _spriteBatch;

        public Version(
            SpriteFont font,
            Viewport viewport,
            SpriteBatch spriteBatch)
        {
            _font = font;
            _viewport = viewport;
            _spriteBatch = spriteBatch;

            var size = _font.MeasureString(V);
            _position = new Vector2(_viewport.Width - size.X, _viewport.Height - size.Y);
        }

        public void Draw(GameTime time)
        {
            _spriteBatch.DrawString(_font, V, _position, Color.White);
        }
    }
}
