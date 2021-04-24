using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Audio;

namespace KenneyAsteroids.Engine.Audio
{
    public sealed class SoundSystem : IAudioPlayer
    {
        private readonly IOptionsMonitor<AudioSettings> _settings;

        public SoundSystem(IOptionsMonitor<AudioSettings> settings)
        {
            _settings = settings;
        }

        public void Play(SoundEffect sfx)
        {
            sfx.Play(_settings.CurrentValue.SfxVolume, 0.0f, 0.0f);
        }
    }
}
