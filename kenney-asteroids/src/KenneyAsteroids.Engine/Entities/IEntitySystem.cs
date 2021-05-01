using System.Collections.Generic;

namespace KenneyAsteroids.Engine.Entities
{
    public interface IEntitySystem : IEnumerable<IEntity>
    {
        void Add(params IEntity[] entities);
        void Remove(params IEntity[] entities);

        void Commit();
    }
}