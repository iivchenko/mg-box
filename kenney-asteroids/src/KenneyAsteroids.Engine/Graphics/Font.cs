using System;

namespace KenneyAsteroids.Engine.Graphics
{
    public sealed class Font
    {
        public Font(int lineSpacing)
        {
            Id = Guid.NewGuid();

            LineSpacing = lineSpacing;
        }

        public Guid Id { get; }

        public int LineSpacing { get; }
    }
}
