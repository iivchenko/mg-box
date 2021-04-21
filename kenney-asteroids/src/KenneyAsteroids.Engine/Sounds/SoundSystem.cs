using Microsoft.Xna.Framework.Audio;

namespace KenneyAsteroids.Engine.Audio
{
    public sealed class SoundSystem : IAudioPlayer
    {
        public void Play(SoundEffect sfx)
        {
            sfx.Play();
        }
    }
}
