using Microsoft.Xna.Framework;

namespace Bricks.Desktop.Engine
{
    public interface IEntity
    {
        Vector2 Position { get; }

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
