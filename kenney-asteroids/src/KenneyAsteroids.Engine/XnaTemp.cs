using KenneyAsteroids.Engine.Graphics;

namespace KenneyAsteroids.Engine
{
    public static class XnaTemp
    {
        public static Microsoft.Xna.Framework.Vector2 ToXna(this Vector vector)
        {
            return new Microsoft.Xna.Framework.Vector2(vector.X, vector.Y);
        }

        public static Microsoft.Xna.Framework.Color ToXna(this Color color)
        {
            return new Microsoft.Xna.Framework.Color(color.Red, color.Green, color.Blue, color.Alpha);
        }
    }
}
