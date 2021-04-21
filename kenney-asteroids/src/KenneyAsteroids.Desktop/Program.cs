using Comora;
using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Audio;
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
                .WithServices(container =>
                    {
                        container
                            // TODO: Move file name to configuraiton or to some const
                            .AddSingleton<IRepository<GameSettings>>(_ => new JsonRepository<GameSettings>("game-settings.json"))
                            .Decorate<IRepository<GameSettings>, DefaultInitializerRepositoryDecorator<GameSettings>>()
                            .AddSingleton<IEntitySystem, EntitySystem>()                            
                            .AddSingleton<IAudioPlayer, SoundSystem>()
                            .AddSingleton<IViewport, Viewport>(_ => new Viewport(0.0f, 0.0f, 3840.0f, 2160.0f))
                            .AddSingleton<ICamera, Camera>()
                            .AddSingleton<IEventHandler<EntityCreatedEvent>, EntityCreatedEventHandler>()
                            .AddDrawSystem()
                            .AddEventBus();
                    })
                .WithConfiguration(config =>
                    {
                        config.FullScreen = false;
                        config.IsMouseVisible = true;
                        config.ContentPath = "Content";
                        config.ScreenColor = Color.Black;
                    })
                .Build<BootstrapScreen<MainMenuScreen>>()
                .RunSafe();
        }
    }
}
