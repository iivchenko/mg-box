using System.ComponentModel.DataAnnotations;

namespace KenneyAsteroids.Engine.Audio
{
    public sealed class AudioSettings
    {
        [Range(0, 1, ErrorMessage = "Sound effect volume must be between {1} and {2}!")]
        public float SfxVolume { get; set; }

        [Range(0, 1, ErrorMessage = "Music volume must be between {1} and {2}!")]
        public float MusicVolume { get; set; }
    }
}
