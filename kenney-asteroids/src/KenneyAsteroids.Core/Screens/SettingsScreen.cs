using KenneyAsteroids.Engine.Screens;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        }

        public ToggleFramerate ToggleFramerate { get; set; }
    }

    // TODO: Convert to 'record' after migrating to .NET 5
    public sealed class ToggleFramerate
    {
        public bool Toggle { get; set; }
    }

    public sealed class SettingsScreen : MenuScreen
    {
        private readonly IRepository<GameSettings> _settingsRepository;

        private readonly MenuEntry _toggleFramerate;
        private readonly MenuEntry _back;

        public SettingsScreen(IServiceProvider container)
            : base("Settings", container)
        {
            _settingsRepository = Container.GetService<IRepository<GameSettings>>();

            var settings = _settingsRepository.Read();

            _toggleFramerate = new MenuEntry($"Frame Rate: {Toggle(settings.ToggleFramerate.Toggle)}");
            _toggleFramerate.Selected += (_, __) =>
            {
                var settings = _settingsRepository.Read();
                settings.ToggleFramerate.Toggle = !settings.ToggleFramerate.Toggle;

                _toggleFramerate.Text = $"Frame Rate: {Toggle(settings.ToggleFramerate.Toggle)}";

                _settingsRepository.Update(settings);
            };

            _back = new MenuEntry("Back");
            _back.Selected += (_, e) => OnCancel(e.PlayerIndex);

            MenuEntries.Add(_toggleFramerate);
            MenuEntries.Add(_back);
        }

        private static string Toggle(bool toggle) => toggle ? "On" : "Off";
    }
}
