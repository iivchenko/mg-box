using System.Numerics;

namespace KenneyAsteroids.Engine.Graphics
{
    public interface IPainter
    {
        void Draw(Sprite sprite, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color);
        void Draw(Sprite sprite, Rectangle rectagle, Color color);
        void Draw(Sprite sprite, Rectangle destination, Rectangle source, Color color);
        void DrawString(Font spriteFont, string text, Vector2 position, Color color);
        void DrawString(Font spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale);
    }
}
