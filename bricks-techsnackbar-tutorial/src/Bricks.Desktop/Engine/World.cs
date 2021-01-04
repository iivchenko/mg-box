using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Bricks.Desktop.Engine
{
    public sealed class World : IWorld
    {
        private readonly List<Entity> _entities;

        public World()
        {
            _entities = new List<Entity>();
        }

        public void Add(Entity entity)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
        }

        public IEnumerable<Entity> Move(Entity entity, GameTime gameTime)
        {
            entity.Position += entity.Velocity;// * (float)gameTime.ElapsedGameTime.TotalSeconds;

            var collisions = 
                _entities
                    .Where(x => x != entity)
                    .Where(x => x.Body.Intersects(entity.Body));

            foreach(var collision in collisions.ToList())
            {
                collision.CollidedBy(entity);
            }

            return collisions;
        }

        public void Remove(Entity entity)
        {
            if (_entities.Contains(entity))
            {
                _entities.Remove(entity);
            }
        }
    }
}