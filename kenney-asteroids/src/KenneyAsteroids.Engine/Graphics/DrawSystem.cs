using System;
using Microsoft.Extensions.DependencyInjection;

using XVector = Microsoft.Xna.Framework.Vector2;
using XColor = Microsoft.Xna.Framework.Color;
using XRectangle = Microsoft.Xna.Framework.Rectangle;
using XFont = Microsoft.Xna.Framework.Graphics.SpriteFont;
using XSpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using XSpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

namespace KenneyAsteroids.Engine.Graphics
{
    public sealed class DrawSystem : IDrawSystem
    {
        private readonly Lazy<XSpriteBatch> _batch;

        public DrawSystem(IServiceProvider container)
        {
            _batch = new Lazy<XSpriteBatch>(() => container.GetService<XSpriteBatch>());
        }

        public void Draw(Sprite sprite, XVector position, XVector origin, XVector scale, float rotation, XColor color)
        {
            _batch.Value.Begin();
            _batch
                .Value
                .Draw(
                    sprite.Texture,
                    position,
                    sprite.SourceRectangle,
                    color,
                    rotation,
                    origin,
                    scale,
                    XSpriteEffects.None,
                    0);
            _batch.Value.End();
        }

        public void Draw(Sprite sprite, XRectangle rectagle, XColor color)
        {
            _batch.Value.Begin();
            _batch
                .Value
                .Draw(
                    sprite.Texture,
                    rectagle,
                    color);
            _batch.Value.End();
        }

        public void DrawString(XFont spriteFont, string text, XVector position, XColor color)
        {
            _batch.Value.Begin();
            _batch.Value.DrawString(spriteFont, text, position, color);
            _batch.Value.End();
        }
    }
}
