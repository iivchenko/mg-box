using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Bricks.Desktop.Engine
{
    // TODO: Create physics body, static body, rigid body, kinematic body
    public abstract class Entity : IEntity
    {
        private readonly IWorld _world;

        protected Entity(IWorld world)
        {
            _world = world;

            _world.Add(this);
        }

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Rectangle Body { get; set; }

        public abstract void Draw(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        internal abstract void CollidedBy(Entity entity);

        protected IEnumerable<Entity> Move(GameTime gameTime)
        {
            return _world.Move(this, gameTime);
        }
    }
}
