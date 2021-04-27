using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public interface IEntityFactory
    {
        Asteroid CreateAsteroid(Vector2 position, Vector2 velocity, float rotationSpeed);
        Ship CreateShip(Vector2 position);
    }
}