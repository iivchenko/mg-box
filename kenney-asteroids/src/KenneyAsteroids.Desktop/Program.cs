﻿using KenneyAsteroids.Core.Screens;
using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Eventing.Eventing;
using KenneyAsteroids.Engine.Audio;
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
                            .AddTransient<IRepository<GameSettings>>(_ => new DefaultInitializerRepositoryDecorator<GameSettings>(new JsonRepository<GameSettings>("game-settings.json")))
                            .AddSingleton<IEntitySystem, EntitySystem>()
                            .AddDrawSystem()
                            .AddSingleton<IAudioPlayer, SoundSystem>()
                            .AddSingleton<IEventHandler<EntityCreatedEvent>, EntityCreatedEventHandler>()
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
