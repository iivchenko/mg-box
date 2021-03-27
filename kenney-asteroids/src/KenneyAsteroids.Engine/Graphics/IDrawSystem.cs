using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine.Graphics
{
    public interface IDrawSystem
    {
        void Draw(Texture2D texture, Rectangle target, Color color);
        void Draw(Sprite sprite, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color);
        void Draw(Sprite sprite, Rectangle rectagle, Color color);
        void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color);
        void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
    }
}
