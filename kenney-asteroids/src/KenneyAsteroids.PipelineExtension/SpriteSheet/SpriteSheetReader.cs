using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Graphics;
using System.Collections.Generic;

namespace KenneyAsteroids.PipelineExtension.SpriteSheet
{
    public sealed class SpriteSheetReader : ContentTypeReader<Engine.Graphics.SpriteSheet>
    {
        protected override Engine.Graphics.SpriteSheet Read(ContentReader input, Engine.Graphics.SpriteSheet existingInstance)
        {
            var path = System.IO.Path.GetDirectoryName(input.AssetName);
            var texture = input.ContentManager.Load<Texture2D>($"{path}\\{input.ReadString()}");
            var lenght = input.ReadInt32();
            var sprites = new Dictionary<string, Sprite>(lenght);

            for(var i = 0; i < lenght; i++)
            {
                var name = input.ReadString();
                var rect = new Rectangle
                {
                    X = input.ReadInt32(),
                    Y = input.ReadInt32(),
                    Width = input.ReadInt32(),
                    Height = input.ReadInt32()
                };

                var sprite = new Sprite(texture, rect);

                sprites.Add(name, sprite);
            };

            return new Engine.Graphics.SpriteSheet(sprites);
        }
    }
}
