using KenneyAsteroids.Engine.Screens;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System.IO;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class BootstrapScreen<TStartScreen> : GameScreen
        where TStartScreen : GameScreen, new()
    {
        public override void Initialize()
        {
            base.Initialize();

            var content = ScreenManager.Game.Content;
            var index = content.RootDirectory.Length + 1;

            foreach (var path in Directory.GetFiles(content.RootDirectory, "*", SearchOption.AllDirectories))
            {
                var relativePath = path.Substring(index);
                var file = relativePath.Substring(0, relativePath.Length - 4);

                content.Load<object>(file);
            }

            ScreenManager.Container.GetService<IRepository<GameSettings>>();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            LoadingScreen.Load(ScreenManager, false, null, new TStartScreen());
        }
    }
}
