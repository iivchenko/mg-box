using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Engine
{
    public sealed class Game : Microsoft.Xna.Framework.Game, IScreenSystem
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly GameConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly List<GameScreen> _screens;
        private readonly InputState _input;
        private readonly Type _initialScreen;
        
        private IServiceProvider _container;
        private IDrawSystemBatcher _batch;
        private Color _clearColor;

        public Game(IServiceCollection services, GameConfiguration configuration, Type initialScreen)
        {
            _services = services;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _initialScreen = initialScreen ?? throw new ArgumentNullException(nameof(initialScreen));

            _graphics = new GraphicsDeviceManager(this);
            _screens = new List<GameScreen>();
            _input = new InputState();

            // we must set EnabledGestures before we can query for them, but
            // we don't assume the game wants to read them.
            TouchPanel.EnabledGestures = GestureType.None;
        }

        protected override void Initialize()
        {
            Content.RootDirectory = _configuration.ContentPath;
            IsMouseVisible = _configuration.IsMouseVisible;
            
            _clearColor = _configuration.ScreenColor;

            _services.AddSingleton(Content);
            _services.AddSingleton(GraphicsDevice);
            _services.AddSingleton(new SpriteBatch(GraphicsDevice));

            var options = new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            };

            _container = _services.BuildServiceProvider(options);
#if DEBUG
            _graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width * (2.0 / 3.0));
            _graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height * (2.0 / 3.0));
#elif RELEASE
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
#endif
            _graphics.ApplyChanges();

            base.Initialize();

            _batch = _container.GetService<IDrawSystemBatcher>();

            // TODO: Move Screens to the container and remove reflectrion
            var screen = (GameScreen)Activator.CreateInstance(_initialScreen, _container);

            Add(screen, null);
        }

        public IEnumerable<GameScreen> GetScreens()
        {
            return _screens.ToArray();
        }

        public void Add(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenSystem = this;
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
        public void Remove(GameScreen screen)
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

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Read the keyboard and gamepad.
            _input.Update();

            var otherScreenHasFocus = !IsActive;
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
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(_clearColor);

            _batch.Begin();

            _screens
                .Where(screen => screen.ScreenState != ScreenState.Hidden)
                .Iter(screen => screen.Draw(time));

            _batch.End();
        }
    }
}
