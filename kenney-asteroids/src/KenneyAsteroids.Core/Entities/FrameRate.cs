using KenneyAsteroids.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class FrameRate : IEntity, Engine.IDrawable
    {
        private readonly SpriteFont _font;
        private readonly Viewport _viewport;
        private readonly SpriteBatch _spriteBatch;

        public FrameRate(
            SpriteFont font,
            Viewport viewport, 
            SpriteBatch spriteBatch)
        {
            _font = font;
            _viewport = viewport;
            _spriteBatch = spriteBatch;
        }

        public void Draw(GameTime time)
        {
            var rate = $"{Math.Round(1 / time.ToDelta())}fps";
            var size = _font.MeasureString(rate);
            var position = new Vector2(_viewport.Width - size.X, size.Y); // TODO: Fix the bug. When I set Y = 0 the simxel font partialy out of screen!

            _spriteBatch.DrawString(_font, rate, position, Color.White);
        }
    }
}
