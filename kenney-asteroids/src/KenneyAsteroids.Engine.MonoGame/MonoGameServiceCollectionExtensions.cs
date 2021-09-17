using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.MonoGame;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MonoGameServiceCollectionExtensions
    {
        public static IServiceCollection AddMonoGameDrawSystem(this IServiceCollection services)
        {
            services.TryAddSingleton<MonoGameDrawSystem>();
            services.TryAdd(new ServiceDescriptor(typeof(IPainter), x => x.GetService<MonoGameDrawSystem>(), ServiceLifetime.Singleton));
            services.TryAdd(new ServiceDescriptor(typeof(IDrawSystemBatcher), x => x.GetService<MonoGameDrawSystem>(), ServiceLifetime.Singleton));
            services.TryAdd(new ServiceDescriptor(typeof(IFontService), x => x.GetService<MonoGameDrawSystem>(), ServiceLifetime.Singleton));

            return services;
        }

        public static IServiceCollection AddMonoGameAudioSystem(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions<AudioSettings>()
                .Bind(configuration)
                .ValidateDataAnnotations();

            services.TryAddSingleton<IAudioPlayer, MonoGameSoundSystem>();

            return services;
        }

        public static IServiceCollection AddMonoGameContentSystem(this IServiceCollection services)
        {
            services.TryAddSingleton<MonoGameContentProvider>();
            services.TryAdd(new ServiceDescriptor(typeof(IContentProvider), x => x.GetService<MonoGameContentProvider>(), ServiceLifetime.Singleton));

            return services;
        }
    }
}
