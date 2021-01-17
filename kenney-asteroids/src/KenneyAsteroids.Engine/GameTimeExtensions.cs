using Microsoft.Xna.Framework;

namespace KenneyAsteroids.Engine
{
    public static class GameTimeExtensions
    {
        public static float ToDelta(this GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
