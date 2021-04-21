using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine.Graphics
{
    public sealed class DrawSystem : IPainter, IDrawSystemBatcher
    {
        private readonly SpriteBatch _batch;

        public DrawSystem(SpriteBatch batch)
        {
            _batch = batch;
        }

        public void Begin()
        {
            _batch.Begin();
        }

        public void End()
        {
            _batch.End();
        }

        public void Draw(Sprite sprite, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color)
        {
            _batch
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
                .Draw(
                    sprite.Texture,
                    rectagle,
                    color);
        }

        public void Draw(Texture2D texture, Rectangle target, Color color)
        {
            _batch.Draw(texture, target, color);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            _batch.DrawString(spriteFont, text, position, color);
        }
        
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _batch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
