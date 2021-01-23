using System.Xml.Serialization;

// TODO: Improve sprite sheet processing
// * Add release logging
// * Add debug logging
// * Think on how merge two input files (sprite and meta) into single one sprite-sheet.xnb (Samples can be found for Monogame font processor)
namespace KenneyAsteroids.PipelineExtension.SpriteSheet
{
    [XmlRoot("TextureAtlas")]
    public sealed class SpriteSheetMeta
    {
        [XmlAttribute("imagePath")]
        public string Texture { get; set; }

        [XmlElement("SubTexture")]
        public SpriteMeta[] Items { get; set; }
    }

    public sealed class SpriteMeta
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("x")]
        public int X { get; set; }

        [XmlAttribute("y")]
        public int Y { get; set; }

        [XmlAttribute("width")]
        public int Width { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }
    }
}
