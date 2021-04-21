namespace KenneyAsteroids.Engine.Graphics
{
    public sealed class Viewport : IViewport
    {
        public Viewport(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float X { get; }

        public float Y { get; }

        public float Width { get; }

        public float Height { get; }
    }
}
