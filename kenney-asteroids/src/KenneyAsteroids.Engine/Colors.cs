namespace KenneyAsteroids.Engine
{
    public static class Colors
    {
        private static readonly Color _white = new Color(255, 255, 255, 255);
        private static readonly Color _turquoise = new Color(64, 224, 208, 255);
        private static readonly Color _black = new Color(0, 0, 0, 255);
        private static readonly Color _red = new Color(255, 0, 0, 255);
        private static readonly Color _yellow = new Color(255, 255, 0, 255);
        private static readonly Color _blue = new Color(0, 0, 255, 255);
        private static readonly Color _darkGray = new Color(169, 169, 169, 255);

        public static Color White => _white;
        public static Color Turquoise => _turquoise;
        public static Color Black => _black;
        public static Color Red => _red;
        public static Color Yellow => _yellow;
        public static Color Blue => _blue;
        public static Color DarkGray => _darkGray;
    }
}
