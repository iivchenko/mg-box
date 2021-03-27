using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Extensions.DependencyInjection;
using KenneyAsteroids.Engine.Graphics;

namespace KenneyAsteroids.Engine.Screens
{
    public sealed class ScreenManager : DrawableGameComponent
    {
        private readonly IServiceProvider _container;
        private readonly List<GameScreen> _screens;
        private readonly InputState _input;

        private IDrawSystemBatcher _batch;

        private bool _traceEnabled;

        public ScreenManager(Game game, IServiceProvider container)
            : base(game)
        {
            _container = container;

            _screens = new List<GameScreen>();
            _input = new InputState();

            // we must set EnabledGestures before we can query for them, but
            // we don't assume the game wants to read them.
            TouchPanel.EnabledGestures = GestureType.None;
        }

        public override void Initialize()
        {
            base.Initialize();

            _batch = _container.GetService<IDrawSystemBatcher>();
        }

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled
        {
            get { return _traceEnabled; }
            set { _traceEnabled = value; }
        }

        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            _input.Update();

            var otherScreenHasFocus = !Game.IsActive;
            var coveredByOtherScreen = false;
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var screen in _screens.ToArray().Reverse())
            {
                screen.Update(time, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(_input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            // Print debug trace?
            if (_traceEnabled)
                TraceScreens();
        }

        public override void Draw(GameTime gameTime)
        {
            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _batch.Begin();

            _screens
                .Where(screen => screen.ScreenState != ScreenState.Hidden)
                .Iter(screen => screen.Draw(time));

            _batch.End();
        }

        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;
            screen.Initialize();

            _screens.Add(screen);

            // update the TouchPanel to respond to gestures this screen is interested in
            TouchPanel.EnabledGestures = screen.EnabledGestures;
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            screen.Free();

            _screens.Remove(screen);

            // if there is a screen still in the manager, update TouchPanel
            // to respond to gestures that screen is interested in.
            if (_screens.Count > 0)
            {
                TouchPanel.EnabledGestures = _screens[_screens.Count - 1].EnabledGestures;
            }
        }
        
        public GameScreen[] GetScreens()
        {
            return _screens.ToArray();
        }

        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        private void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in _screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }
    }
}
