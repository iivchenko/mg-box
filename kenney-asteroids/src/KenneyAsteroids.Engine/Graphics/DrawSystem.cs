using Comora;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace KenneyAsteroids.Engine.Graphics
{
    public sealed class DrawSystem : IPainter, IDrawSystemBatcher
    {
        private readonly SpriteBatch _batch;
        private readonly ICamera _camera;

        public DrawSystem(SpriteBatch batch, ICamera camera)
        {
            _batch = batch;
            _camera = camera;
        }

        public void Begin()
        {
            _batch.Begin(_camera);
        }

        public void End()
        {
            _batch.End();
        }

        public void Draw(Sprite sprite, Vector2 position, Vector2 origin, Vector2 scale, float rotation, Color color)
        {
            _batch
                .Draw(
                    sprite.Texture,
                    position.ToXnaVector(),
                    sprite.SourceRectangle,
                    color.ToXna(),
                    rotation,
                    origin.ToXnaVector(),
                    scale.ToXnaVector(),
                    SpriteEffects.None,
                    0);
        }

        public void Draw(Sprite sprite, Rectangle rectagle, Color color)
        {
            _batch
                .Draw(
                    sprite.Texture,
                    rectagle,
                    color.ToXna());
        }

        public void Draw(Texture2D texture, Rectangle target, Rectangle source, Color color)
        {
            _batch
                .Draw(
                    texture,
                    target,
                    source,
                    color.ToXna());
        }

        public void Draw(Texture2D texture, Rectangle target, Color color)
        {
            _batch.Draw(texture, target, color.ToXna());
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            _batch.DrawString(spriteFont, text, position.ToXnaVector(), color.ToXna());
        }
        
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _batch.DrawString(spriteFont, text, position.ToXnaVector(), color.ToXna(), rotation, origin.ToXnaVector(), scale, effects, layerDepth);
        }
    }
}
