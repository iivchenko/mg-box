using KenneyAsteroids.Engine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KenneyAsteroids.Engine
{
    public sealed class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;

        public Game(GameScreen initialScreen)
        {
            _graphics = new GraphicsDeviceManager(this);
            _screenManager = new ScreenManager(this);

            _screenManager.AddScreen(initialScreen, null);

            Components.Add(_screenManager);

            ScreenColor = Color.CornflowerBlue;
        }

        public Color ScreenColor { get; set; }

        protected override void Initialize()
        {
#if DEBUG
            _graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width * (2.0 / 3.0));
            _graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height * (2.0 / 3.0));
            _graphics.ApplyChanges();
#elif RELEASE
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
#endif

            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ScreenColor);

            base.Draw(gameTime);
        }
    }
}
