using System.Numerics;

namespace KenneyAsteroids.Engine.Collisions
{
    public interface IBody
    {
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
