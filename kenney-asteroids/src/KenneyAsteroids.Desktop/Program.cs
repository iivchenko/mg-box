using Comora;
using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Core.Leaderboards;
using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace KenneyAsteroids.Desktop
{
    public static class Program
    {
        private const string ConfigFile = "game-settings.json";
        private const string LeaderBoardsFile = "leaders.json";

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
                            .AddSingleton<IRepository<Collection<LeaderboardItem>>>(_ => new JsonRepository<Collection<LeaderboardItem>>(LeaderBoardsFile))
                            .Decorate<IRepository<Collection<LeaderboardItem>>, DefaultInitializerRepositoryDecorator<Collection<LeaderboardItem>>>()
                            .AddSingleton<IEntitySystem, EntitySystem>()
                            .AddSingleton<IEntityFactory, EntityFactory>()
                            .AddSingleton<IProjectileFactory, ProjectileFactory>()
                            .AddSingleton<IViewport, Viewport>(_ => new Viewport(0.0f, 0.0f, 3840.0f, 2160.0f))
                            .AddSingleton<ICamera, Camera>()
                            .AddMonoGameContentSystem()
                            .AddMonoGameDrawSystem()
                            .AddMonoGameAudioSystem(configuration.GetSection("Audio"))
                            .AddGameRules(new[] { Assembly.GetAssembly(typeof(Core.Version)) })
                            .AddSingleton<LeaderboardsManager>();
                    })
                .WithConfiguration(config => // TODO: This beast seems become redundant
                    {
                        config.FullScreen = false; // TODO: Make as a part of graphics settings?
                        config.IsMouseVisible = false; // TODO: Make as a part of input settings?
                        config.ContentPath = "Content"; // TODO: Make as a part of content settings?
                        config.ScreenColor = Colors.Black; // TODO: Make as a part of graphics settings?
                    })
                .Build((services, config) => new MonoGameGame(services, config, new BootstrapScreen<MainMenuScreen>()))
                .Run();
        }
    }
}
