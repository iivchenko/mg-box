using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Engine;
using System;

namespace KenneyAsteroids.Desktop
{
    /* TODO: Global tasks
     * Introduce content file conventions: 
     *   Textures -> filename.texture
     *   Spirtes -> filename.sprite
     *   Audion -> filename.audio
     *   Music -> filename.music
    */
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
