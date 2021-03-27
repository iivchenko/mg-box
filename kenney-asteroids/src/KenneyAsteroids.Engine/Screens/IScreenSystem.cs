using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KenneyAsteroids.Engine.Screens
{
    // TODO: Refine interface
    public interface IScreenSystem
    {
        IEnumerable<GameScreen> GetScreens();

        void Add(GameScreen screen, PlayerIndex? controllingPlayer);

        void Remove(GameScreen screen);

        void ResetElapsedTime(); // TODO: used in loading screen. Refactor in more efficient way content loading stuff

        void Exit(); // TODO: Refactor to use event
    }
}