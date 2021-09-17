using KenneyAsteroids.Engine.Audio;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;

using XMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;

namespace KenneyAsteroids.Core.Screens
{
    // TODO: Convert to 'record' after migrating to .NET 5
    public sealed class GameSettings
    {
        public GameSettings()
        {
            ToggleFramerate = new ToggleFramerate
            {
                Toggle = false
            };

            Audio = new AudioSettings();
        }

        public AudioSettings Audio { get; set; }

        public ToggleFramerate ToggleFramerate { get; set; }
    }

    // TODO: Convert to 'record' after migrating to .NET 5
    public sealed class ToggleFramerate
    {
        public bool Toggle { get; set; }
    }

    public sealed class SettingsScreen : MenuScreen
    {
        private IRepository<GameSettings> _settingsRepository;
        private MenuEntry _toggleFramerate;
        private MenuEntry _sfxVolume;
        private MenuEntry _musicVolume;
        private MenuEntry _back;

        public SettingsScreen()
            : base("Settings")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _settingsRepository = ScreenManager.Container.GetService<IRepository<GameSettings>>();
            var fontService = ScreenManager.Container.GetService<IFontService>();

            var settings = _settingsRepository.Read();

            var content = ScreenManager.Container.GetService<IContentProvider>();
            var font = content.Load<Font>("Fonts/kenney-future.h2.font");

            _toggleFramerate = new MenuEntry($"Frame Rate: {Toggle(settings.ToggleFramerate.Toggle)}", font, fontService);
            _toggleFramerate.Selected += (_, __) =>
            {
                var settings = _settingsRepository.Read();
                settings.ToggleFramerate.Toggle = !settings.ToggleFramerate.Toggle;

                _toggleFramerate.Text = $"Frame Rate: {Toggle(settings.ToggleFramerate.Toggle)}";

                _settingsRepository.Update(settings);
            };

            _sfxVolume = new MenuEntry($"Sound Effect Volume: {(int)(settings.Audio.SfxVolume * 100)}%", font, fontService);
            _sfxVolume.Selected += (_, __) =>
            {
                var settings = _settingsRepository.Read();
                settings.Audio.SfxVolume = (float)System.Math.Round(settings.Audio.SfxVolume + 0.1f, 1);

                if (settings.Audio.SfxVolume > 1.0f)
                    settings.Audio.SfxVolume = 0.0f;

                _sfxVolume.Text = $"Sound Effect Volume: {(int)(settings.Audio.SfxVolume * 100)}%";

                _settingsRepository.Update(settings);

                XMediaPlayer.Volume = settings.Audio.MusicVolume;
            };

            _musicVolume = new MenuEntry($"Music Volume: {(int)(settings.Audio.MusicVolume * 100)}%", font, fontService);
            _musicVolume.Selected += (_, __) =>
            {
                var settings = _settingsRepository.Read();
                settings.Audio.MusicVolume = (float)System.Math.Round(settings.Audio.MusicVolume + 0.1f, 1);

                if (settings.Audio.MusicVolume > 1.0f)
                    settings.Audio.MusicVolume = 0.0f;

                _musicVolume.Text = $"Music Volume: {(int)(settings.Audio.MusicVolume * 100)}%";

                _settingsRepository.Update(settings);
            };

            _back = new MenuEntry("Back", font, fontService);
            _back.Selected += (_, e) => OnCancel(e.PlayerIndex);

            MenuEntries.Add(_toggleFramerate);
            MenuEntries.Add(_sfxVolume);
            MenuEntries.Add(_musicVolume);
            MenuEntries.Add(_back);
        }

        private static string Toggle(bool toggle) => toggle ? "On" : "Off";
    }
}
