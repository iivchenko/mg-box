using KenneyAsteroids.Engine.Graphics;
using System.Numerics;

namespace KenneyAsteroids.Engine.Particles
{
    public class Particle
    {
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public Vector2 Scale { get; set; }
        public float TTL { get; set; }
    }
}
