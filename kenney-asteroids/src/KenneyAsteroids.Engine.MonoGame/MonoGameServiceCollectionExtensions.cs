using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.MonoGame;
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

            return services;
        }
    }
}
