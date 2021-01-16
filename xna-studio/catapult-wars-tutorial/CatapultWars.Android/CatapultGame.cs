using CatapultWars.Android.Screens;
using CatapultWars.Android.Utility;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CatapultWars.Android
{
    public class CatapultGame : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public CatapultGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            //Create a new instance of the Screen Manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            //Switch to full screen for best game experience
            graphics.IsFullScreen = true;

            // Add main menu and background
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

            AudioManager.Initialize(this);
        }

        protected override void LoadContent()
        {
            AudioManager.LoadSounds();
            base.LoadContent();
        }
    }
}
