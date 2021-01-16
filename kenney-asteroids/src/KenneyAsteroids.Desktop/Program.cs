using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Engine;
using System;

namespace KenneyAsteroids.Desktop
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new Game(new GamePlayScreen()))
            {
                game.IsMouseVisible = true;
                game.Content.RootDirectory = "Content";
                game.ScreenColor = Microsoft.Xna.Framework.Color.Black;

                game.Run();
            }
        }
    }
}
