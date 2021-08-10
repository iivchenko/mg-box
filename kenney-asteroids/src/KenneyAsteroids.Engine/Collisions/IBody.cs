using System.Numerics;

using Color = Microsoft.Xna.Framework.Color;

namespace KenneyAsteroids.Engine.Collisions
{
    public interface IBody
    {
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Color[] Data { get; set; }
    }
}
