using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Engine
{
    public sealed class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;
        private readonly IServiceCollection _services;

        private IServiceProvider _container;

        private readonly GameConfiguration _configuration;

        private readonly Type _initialScreen;

        public Game(IServiceCollection services, GameConfiguration configuration, Type initialScreen)
        {
            _services = services;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _initialScreen = initialScreen ?? throw new ArgumentNullException(nameof(initialScreen));

            _graphics = new GraphicsDeviceManager(this);
            _screenManager = new ScreenManager(this);

            Components.Add(_screenManager);

            ScreenColor = Color.CornflowerBlue;
        }

        public Color ScreenColor { get; set; } // TODO: Remove

        protected override void Initialize()
        {
            Content.RootDirectory = _configuration.ContentPath;
            IsMouseVisible = _configuration.IsMouseVisible;
            ScreenColor = _configuration.ScreenColor;

            _services.AddSingleton(new SpriteBatch(GraphicsDevice));

            var options = new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            };

            _container = _services.BuildServiceProvider(options);

            // TODO: Move Screens to the container and remove reflectrion
            var screen = (GameScreen)Activator.CreateInstance(_initialScreen, _container);

            _screenManager.AddScreen(screen, null);

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
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ScreenColor);

            base.Draw(gameTime);
        }
    }
}
