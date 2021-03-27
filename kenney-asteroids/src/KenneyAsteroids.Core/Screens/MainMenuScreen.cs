using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class MainMenuScreen : MenuScreen
    {
        private readonly IDrawSystem _draw;

        private SpriteFont _font;
        private string _version;
        private Vector2 _versionPosition;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen(IServiceProvider container)
            : base("Main Menu", container)
        {
            _draw = Container.GetService<IDrawSystem>();
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _font = Content.Load<SpriteFont>("Fonts/simxel.font");

            _version = Version.Current;

            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var size = _font.MeasureString(_version);
            _versionPosition = new Vector2(viewport.Width - size.X, viewport.Height - size.Y);

            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry(_draw, _font, "Play Game");
            MenuEntry settingsMenuEntry = new MenuEntry(_draw, _font, "Settings");
            MenuEntry exitMenuEntry = new MenuEntry(_draw, _font, "Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            settingsMenuEntry.Selected += SettingsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(settingsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void Draw(float time)
        {
            base.Draw(time);
            
            _draw.DrawString(_font, _version, _versionPosition, Color.White);
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, Container, new GamePlayScreen(Container));
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void SettingsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SettingsScreen(Container), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(Microsoft.Xna.Framework.PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?\nA button, Space, Enter = ok\nB button, Esc = cancel";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message, Container);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
