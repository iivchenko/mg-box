using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine.Graphics
{
    public interface IPainter
    {
        void Draw(Sprite sprite, System.Numerics.Vector2 position, System.Numerics.Vector2 origin, System.Numerics.Vector2 scale, float rotation, Color color);
        void Draw(Sprite sprite, Rectangle rectagle, Color color);
        void Draw(Sprite sprite, Rectangle destination, Rectangle source, Color color);
        void DrawString(SpriteFont spriteFont, string text, System.Numerics.Vector2 position, Color color);
        void DrawString(SpriteFont spriteFont, string text, System.Numerics.Vector2 position, Color color, float rotation, System.Numerics.Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
    }
}
