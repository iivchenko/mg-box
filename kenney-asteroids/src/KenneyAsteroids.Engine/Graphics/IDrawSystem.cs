using XVector = Microsoft.Xna.Framework.Vector2;
using XColor = Microsoft.Xna.Framework.Color;
using XRectangle = Microsoft.Xna.Framework.Rectangle;
using XFont = Microsoft.Xna.Framework.Graphics.SpriteFont;

namespace KenneyAsteroids.Engine.Graphics
{
    public interface IDrawSystem
    {
        void Draw(Sprite sprite, XVector position, XVector origin, XVector scale, float rotation, XColor color);
        void Draw(Sprite sprite, XRectangle rectagle, XColor color);
        void DrawString(XFont spriteFont, string text, XVector position, XColor color);
    }
}
