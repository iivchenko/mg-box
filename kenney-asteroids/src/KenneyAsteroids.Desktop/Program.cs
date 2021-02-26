using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine;
using System;

namespace KenneyAsteroids.Desktop
{
    /* TODO: Global tasks
     * 
     * Implement pipeline processor for Kenney's Sprite Sheets 
     *  - http://rbwhitaker.wikidot.com/content-pipeline-extension-7
     * Introduce content file conventions: 
     *   Textures -> filename.texture
     *   Spirtes -> filename.sprite
     *   Audion -> filename.audio
     *   Music -> filename.music
     * Abstract MonoGame: Remove dependencier from the CORE, leave depedencie in the Engine
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
