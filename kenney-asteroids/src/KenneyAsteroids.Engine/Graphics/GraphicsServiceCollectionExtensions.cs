using KenneyAsteroids.Engine.Graphics;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GraphicsServiceCollectionExtensions
    {
        public static IServiceCollection AddDrawSystem(this IServiceCollection services)
        {
            services.TryAddSingleton<DrawSystem>();
            services.TryAdd(new ServiceDescriptor(typeof(IPainter), x => x.GetService<DrawSystem>(), ServiceLifetime.Singleton));
            services.TryAdd(new ServiceDescriptor(typeof(IDrawSystemBatcher), x => x.GetService<DrawSystem>(), ServiceLifetime.Singleton));

            return services;
        }
    }
}
