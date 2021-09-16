using Comora;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

using XTime = Microsoft.Xna.Framework.GameTime;
using XVector = Microsoft.Xna.Framework.Vector2;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class BootstrapScreen<TStartScreen> : GameScreen
        where TStartScreen : GameScreen, new()
    {
        public override void Initialize()
        {
            base.Initialize();
            
            GameRoot.ScreenManager = ScreenManager;

            var camera = ScreenManager.Container.GetService<ICamera>();
            var view = ScreenManager.Container.GetService<IViewport>();
            var nativeView = ScreenManager.GraphicsDevice.Viewport;
            
            camera.Position = new XVector(view.Width / 2.0f, view.Height / 2.0f);
            camera.Width = view.Width;
            camera.Height = view.Height;
            camera.Zoom = nativeView.Width / view.Width;

            var content = ScreenManager.Game.Content;
            var index = content.RootDirectory.Length + 1;

            foreach (var path in Directory.GetFiles(content.RootDirectory, "*", SearchOption.AllDirectories))
            {
                var relativePath = path.Substring(index);
                var file = relativePath.Substring(0, relativePath.Length - 4);

                content.Load<object>(file);
            }
        }

        public override void Update(XTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            LoadingScreen.Load(ScreenManager, false, null, new StarScreen(), new TStartScreen());
        }
    }
}
