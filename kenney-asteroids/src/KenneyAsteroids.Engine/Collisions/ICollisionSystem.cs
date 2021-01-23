using System.Collections.Generic;

namespace KenneyAsteroids.Engine.Collisions
{
    // TODO: Think on using NRules or somethign similar
    public interface ICollisionSystem
    {
        void ApplyCollisions(IEnumerable<IBody> bodies);
    }
}
