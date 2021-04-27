using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public interface IProjectileFactory
    {
        Projectile Create(Vector2 position, Vector2 direction);
    }
}
