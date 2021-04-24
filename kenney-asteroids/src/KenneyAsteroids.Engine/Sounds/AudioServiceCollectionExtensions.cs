using KenneyAsteroids.Engine.Audio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AudioServiceCollectionExtensions
    {
        public static IServiceCollection AddAudio(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions<AudioSettings>()
                .Bind(configuration)
                .ValidateDataAnnotations();

            services.TryAddSingleton<IAudioPlayer, SoundSystem>();

            return services;
        }
    }
}
