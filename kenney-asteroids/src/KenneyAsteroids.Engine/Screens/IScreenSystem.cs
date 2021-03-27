using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KenneyAsteroids.Engine.Screens
{
    // TODO: Refine interface
    public interface IScreenSystem
    {
        GraphicsDevice GraphicsDevice { get; }

        ContentManager Content { get; }

        IEnumerable<GameScreen> GetScreens();

        void Add(GameScreen screen, PlayerIndex? controllingPlayer);

        void Remove(GameScreen screen);

        void ResetElapsedTime();

        void Exit();
    }
}