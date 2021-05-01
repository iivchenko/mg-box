using System.Collections.Generic;

namespace KenneyAsteroids.Engine.Collisions
{
    public interface ICollisionSystem
    {
        void ApplyCollisions(IEnumerable<IBody> bodies);
    }
}
