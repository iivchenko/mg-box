using CatapultWars.Android.Players;
using CatapultWars.Android.Utility;
using GameStateManagement;
using Microsoft.Xna.Framework;
using System;

namespace CatapultWars.Android.Screens
{
    public sealed class PauseScreen : MenuScreen
    {
        GameScreen backgroundScreen;
        Player human;
        Player computer;
        bool prevHumanIsActive;
        bool prevCompuerIsActive;

        public PauseScreen(GameScreen backgroundScreen, Player human, Player computer)
            : base(String.Empty)
        {
            IsPopup = true;

            this.backgroundScreen = backgroundScreen;

            // Create our menu entries.
            MenuEntry startGameMenuEntry = new MenuEntry("Return");
            MenuEntry exitMenuEntry = new MenuEntry("Quit Game");

            // Hook up menu event handlers.
            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            this.human = human;
            this.computer = computer;

            // Preserve the old state of the game
            prevHumanIsActive = this.human.Catapult.IsActive;
            prevCompuerIsActive = this.computer.Catapult.IsActive;

            // Pause the game logic progress
            this.human.Catapult.IsActive = false;
            this.computer.Catapult.IsActive = false;

            AudioManager.PauseResumeSounds(false);
        }

        void StartGameMenuEntrySelected(object sender, EventArgs e)
        {
            human.Catapult.IsActive = prevHumanIsActive;
            computer.Catapult.IsActive = prevCompuerIsActive;

            if (!(human as Human).isDragging)
                AudioManager.PauseResumeSounds(true);
            else
            {
                (human as Human).ResetDragState();
                AudioManager.StopSounds();
            }

            backgroundScreen.ExitScreen();
            ExitScreen();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            AudioManager.StopSounds();
            ScreenManager.AddScreen(new MainMenuScreen(), null);
            ExitScreen();
        }

        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

            foreach (var entry in MenuEntries)
            {
                Vector2 position = entry.Position;

                position.Y += 60;

                entry.Position = position;
            }
        }

    }
}