using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine.Graphics
{
    public sealed class DrawSystem : IPainter, IDrawSystemBatcher
    {
        private readonly Lazy<SpriteBatch> _batch;

        public DrawSystem(IServiceProvider container)
        {
            _batch = new Lazy<SpriteBatch>(() => container.GetService<SpriteBatch>());
        }

        public void Begin()
        {
            _batch.Value.Begin();
        }

        public void End()
        {
            _batch.Value.End();
        }

        public void Draw(Sprite sprite, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color)
        {
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
                    SpriteEffects.None,
                    0);
        }

        public void Draw(Sprite sprite, Rectangle rectagle, Color color)
        {
            _batch
                .Value
                .Draw(
                    sprite.Texture,
                    rectagle,
                    color);
        }

        public void Draw(Texture2D texture, Rectangle target, Color color)
        {
            _batch.Value.Draw(texture, target, color);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            _batch.Value.DrawString(spriteFont, text, position, color);
        }
        
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _batch.Value.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
