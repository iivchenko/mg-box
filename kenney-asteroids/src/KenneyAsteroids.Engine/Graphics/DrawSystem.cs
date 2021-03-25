using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine.Graphics
{
    public sealed class DrawSystem : IDrawSystem
    {
        private readonly Lazy<SpriteBatch> _batch;

        public DrawSystem(IServiceProvider container)
        {
            _batch = new Lazy<SpriteBatch>(() => container.GetService<SpriteBatch>());
        }

        public void Draw(Sprite sprite, Vector position, Vector origin, Vector scale, float rotation, Color color)
        {
            _batch.Value.Begin();
            _batch
                .Value
                .Draw(
                    sprite,
                    position.ToXna(),
                    origin.ToXna(),
                    scale.ToXna(),
                    rotation,
                    Color.White,
                    SpriteEffects.None);
            _batch.Value.End();
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector position, Color color)
        {
            _batch.Value.Begin();
            _batch.Value.DrawString(spriteFont, text, position.ToXna(), color.ToXna());
            _batch.Value.End();
        }
    }
}
