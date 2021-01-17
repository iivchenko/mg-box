using KenneyAsteroids.Engine.Worlds;
using Microsoft.Xna.Framework;
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

        public static void Update<TEntity>(this IEnumerable<TEntity> entities, GameTime gameTime)
            where TEntity : Entity
        {
            foreach(var entity in entities.Where(x => x is IUpdatable).Cast<IUpdatable>())
            {
                entity.Update(gameTime);
            }
        }

        public static void Draw<TEntity>(this IEnumerable<TEntity> entities, GameTime gameTime)
            where TEntity : Entity
        {
            foreach (var entity in entities.Where(x => x is IDrawable).Cast<IDrawable>())
            {
                entity.Draw(gameTime);
            }
        }
    }
}
