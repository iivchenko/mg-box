using KenneyAsteroids.Engine.Eventing.Eventing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventingServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.TryAddScoped<ServiceFactory>(x => x.GetServices);
            services.TryAddScoped<EventSystem>();
            services.TryAdd(new ServiceDescriptor(typeof(IEventSystem), x => x.GetService<EventSystem>(), ServiceLifetime.Scoped));
            services.TryAdd(new ServiceDescriptor(typeof(IPublisher), x => x.GetService<EventSystem>(), ServiceLifetime.Scoped));

            return services;
        }
    }
}
