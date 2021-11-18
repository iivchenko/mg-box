using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Rules;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GameRulesServiceCollectionExtensions
    {
        public static IServiceCollection AddGameRules(this IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
        {
            services.TryAddSingleton<ServiceFactory>(x => x.GetServices);
            services.TryAddSingleton<GameRuleSystem>();
            services.TryAdd(new ServiceDescriptor(typeof(IGameRuleSystem), x => x.GetService<GameRuleSystem>(), ServiceLifetime.Singleton));
            services.TryAdd(new ServiceDescriptor(typeof(IEventPublisher), x => x.GetService<GameRuleSystem>(), ServiceLifetime.Singleton));                      

            assembliesToScan
                .SelectMany(assembly => assembly.DefinedTypes)
                .Where(type => !type.IsGenericType && !type.IsAbstract)
                .Where(type => type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IRule<>)))
                .SelectMany(type => 
                            type
                                .GetInterfaces()
                                .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IRule<>))
                                .Where(@interface => !@interface.IsGenericMethodParameter)
                                .Select(@interface => new { Interface = @interface, Implementation = type }))
                .Select(type => new ServiceDescriptor(type.Interface, type.Implementation, ServiceLifetime.Singleton))
                .Iter(services.Add);

            return services;
        }
    }
}
