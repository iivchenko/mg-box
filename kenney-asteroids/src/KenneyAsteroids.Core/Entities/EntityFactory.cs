using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Rules;
using System;
using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class EntityFactory : IEntityFactory
    {
        private const int TinyAsteroidMinSpeed = 400;
        private const int TinyAsteroidMaxSpeed = 500;
        private const int TinyAsteroidMinRotationSpeed = 25;
        private const int TinyAsteroidMaxRotationSpeed = 75;

        private const int SmallAsteroidMinSpeed = 200;
        private const int SmallAsteroidMaxSpeed = 300;
        private const int SmallAsteroidMinRotationSpeed = 25;
        private const int SmallAsteroidMaxRotationSpeed = 75;

        private const int MediumAsteroidMinSpeed = 100;
        private const int MediumAsteroidMaxSpeed = 200;
        private const int MediumAsteroidMinRotationSpeed = 15;
        private const int MediumAsteroidMaxRotationSpeed = 45;

        private const int BigAsteroidMinSpeed = 50;
        private const int BigAsteroidMaxSpeed = 100;
        private const int BigAsteroidMinRotationSpeed = 5;
        private const int BigAsteroidMaxRotationSpeed = 25;

        private readonly SpriteSheet _spriteSheet;
        private readonly Sound _lazer;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IEventPublisher _publisher;
        private readonly IPainter _draw;
        private readonly IAudioPlayer _player;

        public EntityFactory(
            IContentProvider content,
            IProjectileFactory projectileFactory,
            IEventPublisher eventService,
            IPainter draw,
            IAudioPlayer player)
        {
            _spriteSheet = content.Load<SpriteSheet>("SpriteSheets/Asteroids.sheet");
            _lazer = content.Load<Sound>("Sounds/laser.sound");
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
        public Asteroid CreateAsteroid(AsteroidType type, Vector2 position, float direction)
        {
            var random = new Random();
            String spriteName;
            Sprite sprite;
            int speedX;
            int speedY;
            int rotationSpeed;
            Vector2 velocity;
            float scale;

            switch (type)
            {
                case AsteroidType.Tiny:
                    spriteName = new[] 
                    {
                        "meteorBrown_tiny1",
                        "meteorBrown_tiny2",
                        "meteorGrey_tiny1",
                        "meteorGrey_tiny2"
                    }.RandomPick();
                    sprite = _spriteSheet[spriteName];
                    scale = 3;
                    speedX = random.Next(TinyAsteroidMinSpeed, TinyAsteroidMaxSpeed);
                    speedY = random.Next(TinyAsteroidMinSpeed, TinyAsteroidMaxSpeed);
                    rotationSpeed = random.Next(TinyAsteroidMinRotationSpeed, TinyAsteroidMaxRotationSpeed).AsRadians() * random.NextDouble() > 0.5 ? 1 : -1;
                    velocity = direction.ToDirection() * new Vector2(speedX, speedY);
                    break;

                case AsteroidType.Small:
                    spriteName = new[]
                    {
                        "meteorBrown_small1",
                        "meteorBrown_small2",
                        "meteorGrey_small1",
                        "meteorGrey_small2"
                    }.RandomPick();

                    sprite = _spriteSheet[spriteName];
                    scale = 2.2f;
                    speedX = random.Next(SmallAsteroidMinSpeed, SmallAsteroidMaxSpeed);
                    speedY = random.Next(SmallAsteroidMinSpeed, SmallAsteroidMaxSpeed);
                    rotationSpeed = random.Next(SmallAsteroidMinRotationSpeed, SmallAsteroidMaxRotationSpeed).AsRadians() * random.NextDouble() > 0.5 ? 1 : -1;
                    velocity = direction.ToDirection() * new Vector2(speedX, speedY);
                    break;

                case AsteroidType.Medium:
                    spriteName = new[]
                    {
                        "meteorGrey_med1",
                        "meteorGrey_med2",
                        "meteorBrown_med1",
                        "meteorBrown_med2"
                    }.RandomPick();

                    sprite = _spriteSheet[spriteName];
                    scale = 1.7f;
                    speedX = random.Next(MediumAsteroidMinSpeed, MediumAsteroidMaxSpeed);
                    speedY = random.Next(MediumAsteroidMinSpeed, MediumAsteroidMaxSpeed);
                    rotationSpeed = random.Next(MediumAsteroidMinRotationSpeed, MediumAsteroidMaxRotationSpeed).AsRadians() * random.NextDouble() > 0.5 ? 1 : -1;
                    velocity = direction.ToDirection() * new Vector2(speedX, speedY);
                    break;

                case AsteroidType.Big:
                    spriteName = new[]
                    {
                        "meteorBrown_big1",
                        "meteorBrown_big2",
                        "meteorBrown_big3",
                        "meteorBrown_big4",
                        "meteorGrey_big1",
                        "meteorGrey_big2",
                        "meteorGrey_big3",
                        "meteorGrey_big4"
                    }.RandomPick();

                    sprite = _spriteSheet[spriteName];
                    scale = 1;
                    speedX = random.Next(BigAsteroidMinSpeed, BigAsteroidMaxSpeed);
                    speedY = random.Next(BigAsteroidMinSpeed, BigAsteroidMaxSpeed);
                    rotationSpeed = random.Next(BigAsteroidMinRotationSpeed, BigAsteroidMaxRotationSpeed).AsRadians() * random.NextDouble() > 0.5 ? 1 : -1;
                    velocity = direction.ToDirection() * new Vector2(speedX, speedY);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown asteroid type {type}!");
            }

            return new Asteroid(_draw, type, sprite, velocity, new Vector2(scale), rotationSpeed)
            {
                Position = position
            };
        }
    }
}
