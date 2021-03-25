using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine.Graphics
{
    public interface IDrawSystem
    {
        void Draw(Sprite sprite, Vector position, Vector origin, Vector scale, float rotation, Color color);
        void DrawString(SpriteFont spriteFont, string text, Vector position, Color color);
    }
}
