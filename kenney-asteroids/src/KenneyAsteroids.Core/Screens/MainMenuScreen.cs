using KenneyAsteroids.Core.Screens.GamePlay;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Numerics;

using Color = Microsoft.Xna.Framework.Color;
using XTime = Microsoft.Xna.Framework.GameTime;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class MainMenuScreen : MenuScreen
    {
        private IPainter _painter;
        private SpriteFont _font;
        private string _version;
        private Vector2 _versionPosition;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            _painter = ScreenManager.Container.GetService<IPainter>();
            _font = ScreenManager.Game.Content.Load<SpriteFont>("Fonts/simxel.font");
            _version = Version.Current;

            var viewport = ScreenManager.Container.GetService<IViewport>();
            var size = _font.MeasureString(_version);
            _versionPosition = new Vector2(viewport.Width - size.X, viewport.Height - size.Y);

            // Create our menu entries.
            var playGameMenuEntry = new MenuEntry("Play Game");
            var leaderboardMenuEntry = new MenuEntry("Leaderboard");
            var settingsMenuEntry = new MenuEntry("Settings");
            var exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            leaderboardMenuEntry.Selected += LeaderboardMenuEntrySelected;
            settingsMenuEntry.Selected += SettingsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(leaderboardMenuEntry);
            MenuEntries.Add(settingsMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            if (MediaPlayer.State == MediaState.Stopped)
            {
                var song = ScreenManager.Container.GetService<ContentManager>().Load<Song>("Music/menu.song");
                
                MediaPlayer.Play(song);
            }
        }

        public override void Draw(XTime gameTime)
        {
            base.Draw(gameTime);

            var time = gameTime.ToDelta();

            _painter.DrawString(_font, _version, _versionPosition, Color.White);
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GamePlayScreen());
        }

        void LeaderboardMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new LeaderBoardsScreen());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void SettingsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SettingsScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(Microsoft.Xna.Framework.PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?\nA button, Space, Enter = ok\nB button, Esc = cancel";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

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
