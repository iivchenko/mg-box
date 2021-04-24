using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Messaging;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Audio;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class EntityFactory : IProjectileFactory // TODO: Extract Projectile factory class
    {
        private readonly SpriteSheet _spriteSheet;
        private readonly SoundEffect _lazer;
        private readonly IPublisher _eventService;
        private readonly IPainter _draw;
        private readonly IAudioPlayer _player;

        public EntityFactory(
            SpriteSheet spriteSheet,
            SoundEffect lazer,
            IPublisher eventService,
            IPainter draw,
            IAudioPlayer player)
        {
            _spriteSheet = spriteSheet;
            _lazer = lazer;
            _eventService = eventService;
            _draw = draw;
            _player = player;
        }

        public Ship CreateShip(Vector2 position)
        {
            const float MaxSpeed = 400.0f;
            const float Acceleration = 10.0f;
            const float MaxRotation = 180.0f;

            var sprite = _spriteSheet["playerShip1_blue"];
            var reload = TimeSpan.FromMilliseconds(500);
            var weapon = new Weapon(new Vector2(0, -sprite.Width / 2), reload, this, _eventService, _player, _lazer);
            return new Ship(_draw, sprite, weapon, MaxSpeed, Acceleration, MaxRotation.AsRadians())
            {
                Position = position
            };
        }

        public Asteroid CreateAsteroid(Vector2 position, Vector2 velocity, float rotationSpeed)
        {
            var sprite = _spriteSheet["meteorBrown_big2"];
            
            return new Asteroid(_draw, sprite, velocity, rotationSpeed)
            {
                Position = position
            };
        }

        public Projectile Create(Vector2 position, Vector2 direction)
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
