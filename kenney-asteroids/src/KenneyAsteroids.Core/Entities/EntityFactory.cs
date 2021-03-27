using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Eventing.Eventing;
using KenneyAsteroids.Engine.Graphics;
using System;

using XVector = Microsoft.Xna.Framework.Vector2;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class EntityFactory : IProjectileFactory // TODO: Extract Projectile factory class
    {
        private readonly SpriteSheet _spriteSheet;
        private readonly IPublisher _eventService;
        private readonly IPainter _draw;

        public EntityFactory(
            SpriteSheet spriteSheet,
            IPublisher eventService,
            IPainter draw)
        {
            _spriteSheet = spriteSheet;
            _eventService = eventService;
            _draw = draw;
        }

        public Ship CreateShip(XVector position)
        {
            const float MaxSpeed = 400.0f;
            const float Acceleration = 10.0f;
            const float MaxRotation = 180.0f;

            var sprite = _spriteSheet["playerShip1_blue"];
            var reload = TimeSpan.FromMilliseconds(500);
            var weapon = new Weapon(new XVector(0, -sprite.Width / 2), reload, this, _eventService);
            return new Ship(_draw, sprite, weapon, MaxSpeed, Acceleration, MaxRotation.AsRadians())
            {
                Position = position
            };
        }

        public Asteroid CreateAsteroid(XVector position, XVector velocity, float rotationSpeed)
        {
            var sprite = _spriteSheet["meteorBrown_big2"];
            
            return new Asteroid(_draw, sprite, velocity, rotationSpeed)
            {
                Position = position
            };
        }

        public Projectile Create(XVector position, XVector direction)
        {
            const float Speed = 800.0f;
            var sprite = _spriteSheet["laserBlue01"];
            var rotation = direction.ToRotation();

            return new Projectile(_draw, sprite, rotation, Speed)
            {
                Position = position
            };
        }
    }
}
