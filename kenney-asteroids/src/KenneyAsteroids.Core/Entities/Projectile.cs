using KenneyAsteroids.Engine;
using Microsoft.Xna.Framework;
using System;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Projectile : IEntity<Guid>
    {
        public Projectile()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
