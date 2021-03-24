using Microsoft.Xna.Framework;
using System;

namespace KenneyAsteroids.Engine
{
    public static class VectorExtensions
    {
        public static float Length(this Vector vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector Normalize(this Vector vector)
        {
            var length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return new Vector(vector.X / length, vector.Y / length);
        }

        public static Vector Transform(this Vector position, Matrix matrix)
        {
            return new Vector((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41, (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
        }

        public static float ToRotation(this Vector direction)
        {
            return MathF.Atan2(direction.X, -direction.Y);
        }

        public static Vector2 ToXna(this Vector vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
    }
}
