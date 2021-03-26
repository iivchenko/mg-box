using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using System;
using Microsoft.Extensions.DependencyInjection;

using XVector = Microsoft.Xna.Framework.Vector2;
using XColor = Microsoft.Xna.Framework.Color;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class MainMenuScreen : MenuScreen
    {
        private readonly IDrawSystem _draw;

        private string _version;
        private XVector _versionPosition;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen(IServiceProvider container)
            : base("Main Menu", container)
        {
            _draw = Container.GetService<IDrawSystem>();

            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry(_draw, "Play Game");
            MenuEntry settingsMenuEntry = new MenuEntry(_draw, "Settings");
            MenuEntry exitMenuEntry = new MenuEntry(_draw, "Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            settingsMenuEntry.Selected += SettingsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(settingsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _version = Version.Current;

            var font = ScreenManager.Font;
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var size = font.MeasureString(_version);
            _versionPosition = new XVector(viewport.Width - size.X, viewport.Height - size.Y);
        }

        public override void Draw(float time)
        {
            base.Draw(time);
            
            _draw.DrawString(ScreenManager.Font, _version, _versionPosition, XColor.White);
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
