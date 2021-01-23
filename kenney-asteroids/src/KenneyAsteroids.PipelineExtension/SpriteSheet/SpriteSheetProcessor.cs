using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace KenneyAsteroids.PipelineExtension.SpriteSheet
{
    [ContentProcessor(DisplayName = "Sprite Sheet")]
    public sealed class SpriteSheetProcessor : ContentProcessor<string, SpriteSheetMeta>
    {
        public override SpriteSheetMeta Process(string input, ContentProcessorContext context)
        {
            var buffer = Encoding.UTF8.GetBytes(input);

            using (var stream = new MemoryStream(buffer))
            {
                var serializer = new XmlSerializer(typeof(SpriteSheetMeta));

                return (SpriteSheetMeta)serializer.Deserialize(stream);
            }
        }
    }
}
