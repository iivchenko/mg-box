using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine.Graphics
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(
            this SpriteBatch spriteBatch, 
            Sprite sprite, 
            Vector2 position, 
            Vector2 origin,
            Vector2 scale,
            float rotation,
            Color color,
            SpriteEffects effect)
        {
            spriteBatch
                .Draw(
                    sprite.Texture,
                    position,
                    sprite.SourceRectangle,
                    color.ToXna(),
                    rotation,
                    origin,
                    scale,
                    effect,
                    0);
        }
    }
}
