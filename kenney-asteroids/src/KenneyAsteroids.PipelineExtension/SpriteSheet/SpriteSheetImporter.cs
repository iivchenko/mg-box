using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

namespace KenneyAsteroids.PipelineExtension.SpriteSheet
{
    [ContentImporter(".xml", DisplayName = "Sprite Sheet Importer", DefaultProcessor = "SpriteSheetProcessor")]
    public sealed class SpriteSheetImporter: ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllText(filename);
        }
    }
}
