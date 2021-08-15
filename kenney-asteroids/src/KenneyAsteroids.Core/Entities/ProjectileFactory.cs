using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Graphics;
using System.Numerics;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class ProjectileFactory : IProjectileFactory
    {
        private readonly SpriteSheet _spriteSheet;
        private readonly IPainter _draw;

        public ProjectileFactory(
            IContentProvider content,
            IPainter draw)
        {
            _spriteSheet = content.Load<SpriteSheet>("SpriteSheets/Asteroids.sheet");
            _draw = draw;
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
