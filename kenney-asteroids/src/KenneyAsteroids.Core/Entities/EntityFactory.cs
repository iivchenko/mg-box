using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class EntityFactory
    {
        private readonly SpriteSheet _spriteSheet;
        private readonly SpriteBatch _spriteBatch;
        
        public EntityFactory(SpriteSheet spriteSheet, SpriteBatch spriteBatch)
        {
            _spriteSheet = spriteSheet;
            _spriteBatch = spriteBatch;
        }

        public Ship CreateShip(Vector2 position)
        {
            const float MaxSpeed = 400.0f;
            const float Acceleration = 10.0f;

            var sprite = _spriteSheet["playerShip1_blue"];
            return new Ship(sprite, _spriteBatch, MaxSpeed, Acceleration)
            {
                Position = position
            };
        }

        public Asteroid CreateAsteroid(Vector2 position, Vector2 velocity, float rotationSpeed)
        {
            var sprite = _spriteSheet["meteorBrown_big2"];
            
            return new Asteroid(sprite, _spriteBatch, velocity, rotationSpeed)
            {
                Position = position
            };
        }
    }
}
