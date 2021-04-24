using System;
using System.Numerics;

namespace KenneyAsteroids.Engine
{
    public static class MathExtensions
    {
        public static Vector2 ToDirection(this float angle)
        {
            var direction = new Vector2(MathF.Sin(angle), -MathF.Cos(angle));

            return Vector2.Normalize(direction);
        }

        public static float AsRadians(this float angle)
        {
            return Microsoft.Xna.Framework.MathHelper.ToRadians(angle);
        }

        public static float AsRadians(this int angle)
        {
            return Microsoft.Xna.Framework.MathHelper.ToRadians(angle);
        }
    }
}
