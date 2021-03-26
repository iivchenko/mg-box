using System;

using XVector = Microsoft.Xna.Framework.Vector2;
using XMatrix = Microsoft.Xna.Framework.Matrix;

namespace KenneyAsteroids.Engine
{
    public static class VectorExtensions
    {
        public static XVector ToNormalized(this XVector vector)
        {
            var length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return new XVector(vector.X / length, vector.Y / length);
        }

        public static XVector Transform(this XVector position, XMatrix matrix)
        {
            return new XVector((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41, (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
        }

        public static float ToRotation(this XVector direction)
        {
            return MathF.Atan2(direction.X, -direction.Y);
        }
    }
}
