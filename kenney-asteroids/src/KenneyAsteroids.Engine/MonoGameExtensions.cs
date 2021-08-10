using Microsoft.Xna.Framework;

namespace KenneyAsteroids.Engine
{
    public static class MonoGameExtensions
    {
        public static float ToDelta(this GameTime time)
        {
            return (float)time.ElapsedGameTime.TotalSeconds;
        }

        public static Microsoft.Xna.Framework.Color ToXna(this Color color)
        {
            return new Microsoft.Xna.Framework.Color(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static Color ToEngine(this Microsoft.Xna.Framework.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
