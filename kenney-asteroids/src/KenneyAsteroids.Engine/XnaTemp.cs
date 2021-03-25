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

        public static Vector ToEngine(this Microsoft.Xna.Framework.Vector2 vector)
        {
            return new Vector(vector.X, vector.Y);
        }

        public static Color ToEngine(this Microsoft.Xna.Framework.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
