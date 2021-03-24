namespace KenneyAsteroids.Engine.Graphics
{
    public struct Color
    {
        public static readonly Color White = new Color(255, 255, 255, 255);

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public byte Alpha { get; }
    }
}
