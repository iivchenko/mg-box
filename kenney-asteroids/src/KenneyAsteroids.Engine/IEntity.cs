using Microsoft.Xna.Framework;

namespace KenneyAsteroids.Engine
{
    public interface IEntity<TId> : IEntity
    {
        TId Id { get; }
    }

    public interface IEntity
    {
    }
}
