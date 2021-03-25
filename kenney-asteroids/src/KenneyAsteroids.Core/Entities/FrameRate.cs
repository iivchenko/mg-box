using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class FrameRate : IEntity, IDrawable
    {
        private readonly IDrawSystem _draw;
        private readonly SpriteFont _font;
        private readonly Viewport _viewport;

        public FrameRate(
            IDrawSystem draw,
            SpriteFont font,
            Viewport viewport)
        {
            _draw = draw;
            _font = font;
            _viewport = viewport;
        }

        public void Draw(float time)
        {
            var rate = $"{Math.Round(1 / time)}fps";
            var size = _font.MeasureString(rate);
            var position = new Vector(_viewport.Width - size.X, size.Y); // TODO: Fix the bug. When I set Y = 0 the simxel font partialy out of screen!

            _draw.DrawString(_font, rate, position, Color.White);
        }
    }
}
