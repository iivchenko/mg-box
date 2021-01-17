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

            return new Ship(new Sprite(_spriteSheet, new Rectangle(223, 0, 100, 83)), _spriteBatch, MaxSpeed, Acceleration)
            {
                Position = position
            };
        }
    }
}
