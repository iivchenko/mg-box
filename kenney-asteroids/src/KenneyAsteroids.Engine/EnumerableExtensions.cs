using KenneyAsteroids.Engine.Collisions;
using KenneyAsteroids.Engine.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Engine
{
    public static class EnumerableExtensions
    {
        public static void Iter<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach(var item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<IUpdatable> SelectUpdatable<TEntity>(this IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return entities.Where(x => x is IUpdatable).Cast<IUpdatable>();
        }

        public static IEnumerable<IBody> SelectBodies<TEntity>(this IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return entities.Where(x => x is IBody).Cast<IBody>();
        }

        public static IEnumerable<IDrawable> SelectDrawable<TEntity>(this IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return entities.Where(x => x is IDrawable).Cast<IDrawable>();
        }
    }
}
