using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuakeConsole;
using System;
using System.Diagnostics;

namespace KenneyAsteroids.Engine
{
    public sealed class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GameConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly GameScreen _startScreen;
        
        private IServiceProvider _container;
        private Color _clearColor;

        public Game(
            IServiceCollection services, 
            GameConfiguration configuration, 
            GameScreen startScreen)
        {
            _services = services;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _startScreen = startScreen;

            Graphics = new GraphicsDeviceManager(this);
        }

        public GraphicsDeviceManager Graphics { get; private set; }

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

#if DEBUG
            Graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width * (2.0 / 3.0));
            Graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height * (2.0 / 3.0));

            var console = new ConsoleComponent(this)
            {
                Padding = 10.0f,
                FontColor = Colors.White.ToXna(),
                InputPrefixColor = Colors.White.ToXna(),
                BackgroundColor = Colors.DarkGray.ToXna(),
                BottomBorderThickness = 4.0f,
                BottomBorderColor = Colors.Red.ToXna(),
                SelectionColor = Colors.DarkGray.ToXna(),
                LogInput = cmd => Debug.WriteLine(cmd) // Logs input commands to VS output window.
            };
            Components.Add(console);

            _services.AddSingleton(console);

#elif RELEASE
            Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            Graphics.IsFullScreen = true;
#elif ALPHA
            Graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            Graphics.IsFullScreen = true;

            var console = new ConsoleComponent(this)
            {
                Padding = 10.0f,
                FontColor = Color.White,
                InputPrefixColor = Color.White,
                BackgroundColor = Color.DarkGray,
                BottomBorderThickness = 4.0f,
                BottomBorderColor = Color.Red,
                SelectionColor = Color.DarkGray,
                LogInput = cmd => Debug.WriteLine(cmd) // Logs input commands to VS output window.
            };
            Components.Add(console);

            _services.AddSingleton(console);
#endif
            Graphics.ApplyChanges();

            _container = _services.BuildServiceProvider(options);

            var screenManager = new ScreenManager(this, _container);
            screenManager.AddScreen(_startScreen, null);

            Components.Add(screenManager);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_clearColor.ToXna());

            base.Draw(gameTime);
        }
    }
}
