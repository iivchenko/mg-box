using Comora;
using KenneyAsteroids.Core.Events;
using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Messaging;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace KenneyAsteroids.Desktop
{
    public static class Program
    {
        private const string ConfigFile = "game-settings.json";

        [STAThread]
        public static void Main()
        {
            GameBuilder
                .CreateBuilder()
                .WithServices(container =>
                    {
                        var configuration =
                            new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(ConfigFile, optional: true, reloadOnChange: true)
                                .Build();

                        container
                            .AddOptions()
                            .Configure<GameSettings>(configuration)
                            .AddSingleton<IRepository<GameSettings>>(_ => new JsonRepository<GameSettings>(ConfigFile))
                            .Decorate<IRepository<GameSettings>, DefaultInitializerRepositoryDecorator<GameSettings>>()
                            .AddSingleton<IEntitySystem, EntitySystem>()
                            .AddSingleton<IViewport, Viewport>(_ => new Viewport(0.0f, 0.0f, 3840.0f, 2160.0f))
                            .AddSingleton<ICamera, Camera>()
                            .AddSingleton<IMessageHandler<EntityCreatedEvent>, EntityCreatedEventHandler>()
                            .AddDrawSystem()
                            .AddAudio(configuration.GetSection("Audio"))
                            .AddMessageBus();
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
