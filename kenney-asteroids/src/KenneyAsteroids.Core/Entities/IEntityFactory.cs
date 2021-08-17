using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public interface IEntityFactory
    {
        Asteroid CreateAsteroid(AsteroidType type, Vector2 position, float direction);
        Ship CreateShip(Vector2 position);
    }
}