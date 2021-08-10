using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace KenneyAsteroids.Engine.Graphics
{
    public sealed class Sprite
    {
        private readonly Texture2D _texture;
        private readonly Rectangle? _sourceRectangle;

        public Sprite(Texture2D texture, Rectangle? sourceRectangle)
        {
            _texture = texture;
            _sourceRectangle = sourceRectangle;

            if (_sourceRectangle.HasValue)
            {
                Height = _sourceRectangle.Value.Height;
                Width = _sourceRectangle.Value.Width;
            }
            else
            {
                Height = texture.Height;
                Width = texture.Width;
            }
        }

        public Sprite(Texture2D texture)
            : this(texture, null)
        {
        }

        public float Height { get; }
        public float Width { get; }
        internal Texture2D Texture => _texture;
        internal Rectangle? SourceRectangle => _sourceRectangle;

        public Color[] ReadData()
        {
            var data = new Microsoft.Xna.Framework.Color[(int)Width * (int)Height];
            _texture.GetData(0, _sourceRectangle, data, 0, data.Length);

            return data.Select(x => x.ToEngine()).ToArray();
        }
    }
}
