using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
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

        private SpriteFont _font;

        private MenuEntry _toggleFramerate;
        private MenuEntry _back;

        public SettingsScreen(IServiceProvider container)
            : base("Settings", container)
        {
            _settingsRepository = Container.GetService<IRepository<GameSettings>>();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _font = Content.Load<SpriteFont>("Fonts/simxel.font");

            var settings = _settingsRepository.Read();

            _toggleFramerate = new MenuEntry(DrawSystem, _font, $"Frame Rate: {Toggle(settings.ToggleFramerate.Toggle)}");
            _toggleFramerate.Selected += (_, __) =>
            {
                var settings = _settingsRepository.Read();
                settings.ToggleFramerate.Toggle = !settings.ToggleFramerate.Toggle;

                _toggleFramerate.Text = $"Frame Rate: {Toggle(settings.ToggleFramerate.Toggle)}";

                _settingsRepository.Update(settings);
            };

            _back = new MenuEntry(DrawSystem, _font, "Back");
            _back.Selected += (_, e) => OnCancel(e.PlayerIndex);

            MenuEntries.Add(_toggleFramerate);
            MenuEntries.Add(_back);
        }

        private static string Toggle(bool toggle) => toggle ? "On" : "Off";
    }
}
