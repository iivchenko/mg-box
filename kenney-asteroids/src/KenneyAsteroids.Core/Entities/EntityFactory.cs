using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Eventing.Eventing;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class EntityFactory : IProjectileFactory // TODO: Extract Projectile factory class
    {
        private readonly SpriteSheet _spriteSheet;
        private readonly SpriteBatch _spriteBatch;
        private readonly IPublisher _eventService;

        public EntityFactory(
            SpriteSheet spriteSheet, 
            SpriteBatch spriteBatch,
            IPublisher eventService)
        {
            _spriteSheet = spriteSheet;
            _spriteBatch = spriteBatch;
            _eventService = eventService;
        }

        public Ship CreateShip(Vector2 position)
        {
            const float MaxSpeed = 400.0f;
            const float Acceleration = 10.0f;
            const float MaxRotation = 180.0f;

            var sprite = _spriteSheet["playerShip1_blue"];
            var reload = TimeSpan.FromMilliseconds(500);
            var weapon = new Weapon(new Vector2(0, -sprite.Width / 2), reload, this, _eventService);
            return new Ship(sprite, _spriteBatch, weapon, MaxSpeed, Acceleration, MaxRotation.AsRadians())
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

        public Projectile Create(Vector2 position, Vector2 direction)
        {
            const float Speed = 800.0f;
            var sprite = _spriteSheet["laserBlue01"];
            var rotation = direction.ToRotation();

            return new Projectile(sprite, _spriteBatch, rotation, Speed)
            {
                Position = position
            };
        }
    }
}
