using Comora;
using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Eventing.Eventing;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace KenneyAsteroids.Desktop
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            GameBuilder
                .CreateBuilder()
                .WithServices((container, configuration) =>
                    {
                        container
                            .AddOptions()
                            .Configure<GameSettings>(configuration)
                            // TODO: Move file name to configuraiton or to some const
                            .AddSingleton<IRepository<GameSettings>>(_ => new JsonRepository<GameSettings>("game-settings.json"))
                            .Decorate<IRepository<GameSettings>, DefaultInitializerRepositoryDecorator<GameSettings>>()
                            .AddSingleton<IEntitySystem, EntitySystem>()
                            .AddSingleton<IViewport, Viewport>(_ => new Viewport(0.0f, 0.0f, 3840.0f, 2160.0f))
                            .AddSingleton<ICamera, Camera>()
                            .AddSingleton<IEventHandler<EntityCreatedEvent>, EntityCreatedEventHandler>()
                            .AddDrawSystem()
                            .AddAudio(configuration.GetSection("Audio"))
                            .AddEventBus();
                    })
                .WithConfiguration(config => // TODO: This beast seems become redundant
                    {
                        config.FullScreen = false; // TODO: Make as a part of graphics settings?
                        config.IsMouseVisible = false; // TODO: Make as a part of input settings?
                        config.ContentPath = "Content"; // TODO: Make as a part of content settings?
                        config.ScreenColor = Color.Black; // TODO: Make as a part of graphics settings?
                    })
                .Build<BootstrapScreen<MainMenuScreen>>()
                .RunSafe();
        }
    }
}
