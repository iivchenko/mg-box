using System;

namespace KenneyAsteroids.Engine.Collisions
{
    [Serializable]
    public class CollisionException : Exception
    {
        public CollisionException(string message)
            : base(message)
        {
        }
    }
}
