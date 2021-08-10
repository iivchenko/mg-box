namespace KenneyAsteroids.Engine
{
    public sealed class Color
    {
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

        public static Color operator *(Color value, float scale)
        {
            return new Color((byte)(value.Red * scale), (byte)(value.Green * scale), (byte)(value.Blue * scale), (byte)(value.Alpha * scale));
        }
    }
}
