using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Messaging;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class EntityFactory : IEntityFactory
    {
        private readonly SpriteSheet _spriteSheet;
        private readonly SoundEffect _lazer;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IPublisher _publisher;
        private readonly IPainter _draw;
        private readonly IAudioPlayer _player;

        public EntityFactory(
            ContentManager content,
            IProjectileFactory projectileFactory,
            IPublisher eventService,
            IPainter draw,
            IAudioPlayer player)
        {
            _spriteSheet = content.Load<SpriteSheet>("SpriteSheets/Asteroids.sheet");
            _lazer = content.Load<SoundEffect>("Sounds/laser.sound");
            _projectileFactory = projectileFactory;
            _publisher = eventService;
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
            var weapon = new Weapon(new Vector2(0, -sprite.Width / 2), reload, _projectileFactory, _publisher, _player, _lazer);
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
    }
}
