using KenneyAsteroids.Engine.Audio;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Audio;

namespace KenneyAsteroids.Engine.MonoGame
{
    public sealed class MonoGameSoundSystem : IAudioPlayer
    {
        private readonly IOptionsMonitor<AudioSettings> _settings;
        private readonly MonoGameContentProvider _content;

        public MonoGameSoundSystem(
            IOptionsMonitor<AudioSettings> settings,
            MonoGameContentProvider content)
        {
            _settings = settings;
            _content = content;
        }

        public void Play(Sound sound)
        {
            var sfx = _content.Load<SoundEffect>(sound.Id);
            sfx.Play(_settings.CurrentValue.SfxVolume, 0.0f, 0.0f);
        }
    }
}
