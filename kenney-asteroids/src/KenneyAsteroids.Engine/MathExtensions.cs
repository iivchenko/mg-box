using Microsoft.Xna.Framework;
using System;

namespace KenneyAsteroids.Engine
{
    public static class MathExtensions
    {
        public static Vector ToDirection(this float angle)
        {
            var direction = new Vector(MathF.Sin(angle), -MathF.Cos(angle));

            direction.Normalize();

            return direction;
        }

        public static float AsRadians(this float angle)
        {
            return MathHelper.ToRadians(angle);
        }

        public static float AsRadians(this int angle)
        {
            return MathHelper.ToRadians(angle);
        }
    }
}
