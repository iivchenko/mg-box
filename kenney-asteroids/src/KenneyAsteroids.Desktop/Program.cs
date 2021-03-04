using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
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
            GameBuilder
                .CreateBuilder()
                .WithServices(container =>
                    {
                        container
                            // TODO: Move file name to configuraiton
                            // TODO: Add extension methods to wrap repository with cahing one
                            .AddScoped<IRepository<GameSettings>>(_ => new DefaultInitializerRepositoryDecorator<GameSettings>(new JsonRepository<GameSettings>("game-settings.json"))); 
                    })
                .WithConfiguration(config =>
                    {
                        config.FullScreen = false;
                        config.IsMouseVisible = true;
                        config.ContentPath = "Content";
                        config.ScreenColor = Microsoft.Xna.Framework.Color.Black;
                    })
                .WithInitialScreen<MainMenuScreen>()
                .Build()
                .RunSafe();
        }
    }
}
