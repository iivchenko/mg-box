using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayHudScreen : GameScreen
    {
        private readonly IRepository<GameSettings> _settingsRepository;
        private readonly IEntitySystem _entities;

        public GamePlayHudScreen(IServiceProvider container)
            : base(container)
        {
            _settingsRepository = Container.GetService<IRepository<GameSettings>>();
            _entities = Container.GetService<IEntitySystem>();

            IsPopup = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            var draw = Container.GetService<IPainter>();
            var font = Content.Load<SpriteFont>("Fonts/Default");

            var settings = _settingsRepository.Read();

            if (settings.ToggleFramerate.Toggle)
            {
                _entities.Add(new FrameRate(draw, font, GraphicsDevice.Viewport));
            }
        }

        public override void Free()
        {
            base.Free();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
        }

        public override void Update(float time, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(time, otherScreenHasFocus, coveredByOtherScreen);

            _entities.SelectUpdatable().Iter(x => x.Update(time));
            _entities.Commit();
        }

        public override void Draw(float time)
        {
            base.Draw(time);

            _entities.SelectDrawable().Iter(x => x.Draw(time));
        }
    }
}
