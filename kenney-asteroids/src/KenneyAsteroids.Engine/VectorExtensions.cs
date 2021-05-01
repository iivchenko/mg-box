using System;
using System.Numerics;

using XVector = Microsoft.Xna.Framework.Vector2;
using XMatrix = Microsoft.Xna.Framework.Matrix;

namespace KenneyAsteroids.Engine
{
    public static class VectorExtensions
    {
        public static XVector Transform(this XVector position, XMatrix matrix)
        {
            return new XVector((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41, (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
        }

        public static float ToRotation(this Vector2 direction)
        {
            return MathF.Atan2(direction.X, -direction.Y);
        }

        internal static XVector ToXnaVector(this Vector2 vector)
        {
            return new XVector(vector.X, vector.Y);
        }

        internal static Vector2 ToVector(this XVector vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
    }
}
