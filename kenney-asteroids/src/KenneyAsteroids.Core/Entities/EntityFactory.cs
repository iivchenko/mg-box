using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class EntityFactory
    {
        private readonly Texture2D _spriteSheet;
        private readonly SpriteBatch _spriteBatch;
        
        public EntityFactory(Texture2D spriteSheet, SpriteBatch spriteBatch)
        {
            _spriteSheet = spriteSheet;
            _spriteBatch = spriteBatch;
        }

        public Ship CreateShip(Vector2 position)
        {
            const float MaxSpeed = 400.0f;
            const float Acceleration = 10.0f;
            
            // TODO: Create xnb processor for Sprite
            var sourceRec = new Rectangle(223, 0, 100, 83);
            var sprite = new Sprite(_spriteSheet, sourceRec);

            return new Ship(sprite, _spriteBatch, MaxSpeed, Acceleration)
            {
                Position = position
            };
        }

        public Asteroid CreateAsteroid(Vector2 position, Vector2 velocity, float rotationSpeed)
        {
            // TODO: Create xnb processor for Sprite
            var sourceRec = new Rectangle(0, 521, 119, 96);
            var sprite = new Sprite(_spriteSheet, sourceRec);
            
            return new Asteroid(sprite, _spriteBatch, velocity, rotationSpeed)
            {
                Position = position
            };
        }
    }
}
