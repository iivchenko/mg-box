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
        private readonly GameConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly GameScreen _startScreen;
        
        private IServiceProvider _container;
        private Color _clearColor;

        public Game(IServiceCollection services, GameConfiguration configuration, GameScreen startScreen)
        {
            _services = services;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _startScreen = startScreen;

            _graphics = new GraphicsDeviceManager(this);
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
            GraphicsDevice.Clear(_clearColor);

            base.Draw(gameTime);
        }
    }
}
