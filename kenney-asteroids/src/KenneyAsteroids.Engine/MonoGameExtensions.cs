using Microsoft.Xna.Framework;

namespace KenneyAsteroids.Engine
{
    public static class MonoGameExtensions
    {
        public static float ToDelta(this GameTime time)
        {
            return (float)time.ElapsedGameTime.TotalSeconds;
        }
    }
}
