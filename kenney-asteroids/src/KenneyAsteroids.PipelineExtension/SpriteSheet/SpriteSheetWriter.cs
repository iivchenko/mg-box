using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace KenneyAsteroids.PipelineExtension.SpriteSheet
{
    [ContentTypeWriter]
    public sealed class SpriteSheetWriter : ContentTypeWriter<SpriteSheetMeta>
    {
        protected override void Write(ContentWriter output, SpriteSheetMeta value)
        {
            output.Write(value.Texture);
            output.Write(value.Items.Length);

            foreach(var item in value.Items)
            {
                output.Write(item.Name);
                output.Write(item.X);
                output.Write(item.Y);
                output.Write(item.Width);
                output.Write(item.Height);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteSheetReader).AssemblyQualifiedName;
        }
    }
}
