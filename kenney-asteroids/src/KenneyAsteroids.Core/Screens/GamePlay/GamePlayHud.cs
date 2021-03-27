using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayHud : IEntity, Engine.IDrawable
    {
        private readonly IRepository<GameSettings> _settingsRepository;
        private readonly FrameRate _rate;

        public GamePlayHud(IServiceProvider container)
        {
            _settingsRepository = container.GetService<IRepository<GameSettings>>();

            var settings = _settingsRepository.Read();

            if (settings.ToggleFramerate.Toggle)
            {
                var device = container.GetService<GraphicsDevice>();
                var content = container.GetService<ContentManager>();
                var draw = container.GetService<IPainter>();
                var font = content.Load<SpriteFont>("Fonts/Default");

                _rate = new FrameRate(draw, font, device.Viewport);
            }
        }

        public void Draw(float time)
        {
            _rate?.Draw(time);
        }
    }
}
