namespace KenneyAsteroids.Engine
{
    public struct Vector
    {
        public static readonly Vector Zero = new Vector(0.0f, 0.0f);
        public static readonly Vector One = new Vector(1.0f, 1.0f);

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }

        public float Y { get; }

        public static Vector operator+ (Vector left, Vector right)
        {
            return new Vector(left.X + right.X, left.Y + right.Y);
        }

        public static Vector operator -(Vector left, Vector right)
        {
            return new Vector(left.X - right.X, left.Y - right.Y);
        }

        public static Vector operator* (Vector vector, float scalar)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }

        // TODO: Doesn't look good for me. Check official formulas
        public static Vector operator *(Vector left, Vector right)
        {
            return new Vector(left.X * right.X, left.Y * right.Y);
        }
    }
}
