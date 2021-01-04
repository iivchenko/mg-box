using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Bricks.Desktop.Engine
{
    public interface IWorld
    {
        void Add(Entity entity);

        void Remove(Entity entity);

        IEnumerable<Entity> Move(Entity entity, GameTime gameTime);
    }
}