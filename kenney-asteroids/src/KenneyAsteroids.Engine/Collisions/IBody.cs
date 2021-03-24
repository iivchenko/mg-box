namespace KenneyAsteroids.Engine.Collisions
{
    public interface IBody
    {
        public Vector Position { get; set; }
        public Vector Origin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
