using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Eventing.Eventing;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace KenneyAsteroids.Desktop
{
    /* TODO: Global tasks
     * [DEBUG] Show body physics boundaries
     * [Tech] Abstract MonoGame: Remove dependencier from the CORE, leave depedencie in the Engine
     * [Tech] Port XNA studio ui entites
     * [Tech] Implement pipeline processor for Kenney's Sprite Sheets 
     *  - http://rbwhitaker.wikidot.com/content-pipeline-extension-7
     * [Game] Introduce pixel collision
     * [??] Localization
     * [??][DEBUG] Add in game debug console
     * [??][DEBUG] Add game play snapshot to be able to timeline the entire gameplay
     * For the future project:
        * [Tech] Replace ugly Screen system with better Scene+Layer+Entities system
     * [Tech] Xamarin doesn't support .NET 5 - NO UPGRADE, MONOGAME Content Pipeline still depends on .NET CORE 3.1
     * [Tech] Upgrade to .NET 6 when MonoGame will support .NET 6
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
                            .AddScoped<IRepository<GameSettings>>(_ => new DefaultInitializerRepositoryDecorator<GameSettings>(new JsonRepository<GameSettings>("game-settings.json"))) //TODO: think about Decorate and Scrutor
                            .AddScoped<IEntitySystem, EntitySystem>()
                            .AddScoped<IEventHandler<EntityCreatedEvent>, EntityCreatedEventHandler>()
                            .AddEventBus();
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
