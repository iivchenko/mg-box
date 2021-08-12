using System.Collections.Generic;

namespace KenneyAsteroids.Engine.Collisions
{
    public interface ICollisionSystem
    {
        IEnumerable<Collision> EvaluateCollisions(IEnumerable<IBody> bodies);
    }
}
