using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace KenneyAsteroids.Engine.MonoGame
{
    public sealed class MonoGameContentProvider : IContentProvider
    {
        private readonly ContentManager _content;

        public MonoGameContentProvider(ContentManager content)
        {
            _content = content;
        }

        public TContent Load<TContent>(string path)
            where TContent : class
        {
            var type = typeof(TContent);

            if (type == typeof(Sprite))
            {
                var texture =_content.Load<Texture2D>(path);

                return new Sprite(texture) as TContent;
            }
            else if (type == typeof(SpriteSheet))
            {
                return _content.Load<SpriteSheet>(path) as TContent;
            }
            else if (type == typeof(SoundEffect))
            {
                return _content.Load<SoundEffect>(path) as TContent;
            }
            else if (type == typeof(SpriteFont))
            {
                return _content.Load<SpriteFont>(path) as TContent;
            }
            else if (type == typeof(Song))
            {
                return _content.Load<Song>(path) as TContent;
            }
            else
            {
                throw new System.Exception($"Unknown content type {type.Name}!!");
            }
        }
    }
}
