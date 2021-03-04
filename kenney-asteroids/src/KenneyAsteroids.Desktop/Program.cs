using KenneyAsteroids.Core.Screens;
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
     * [DEBUG] Add in game debug console ???
     * [DEBUG] Add game play snapshot to be able to timeline the entire gameplay???
     * [DEBUG] Show body physics boundaries
     * [DevOps] Add project to CI and make available to download game
     * Introduce pixel collision
     * Abastract game CORE from MonoGame so ENGINE has direct access to MonoGame
     * [TechDept] Introduce DI-Container
    */
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new Game(new MainMenuScreen()))
            {
                game.IsMouseVisible = true;
                game.Content.RootDirectory = "Content";
                game.ScreenColor = Microsoft.Xna.Framework.Color.Black;

                game.Run();
            }
        }
    }
}
