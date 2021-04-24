using KenneyAsteroids.Engine.Messaging;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MessagingServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.TryAddSingleton<ServiceFactory>(x => x.GetServices);
            services.TryAddSingleton<MessageSystem>();
            services.TryAdd(new ServiceDescriptor(typeof(IMessageSystem), x => x.GetService<MessageSystem>(), ServiceLifetime.Singleton));
            services.TryAdd(new ServiceDescriptor(typeof(IPublisher), x => x.GetService<MessageSystem>(), ServiceLifetime.Singleton));

            return services;
        }
    }
}
