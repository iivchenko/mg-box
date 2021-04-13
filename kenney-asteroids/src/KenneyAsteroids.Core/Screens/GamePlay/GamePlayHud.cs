using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayHud : IEntity, Engine.IDrawable
    {
        private readonly IRepository<GameSettings> _settingsRepository;
        private readonly IPainter _painter;
        private readonly SpriteFont _font;
        private readonly Viewport _view;
        private readonly IList<Action<float>> _draws;

        public GamePlayHud(IServiceProvider container)
        {
            _draws = new List<Action<float>>();

            _settingsRepository = container.GetService<IRepository<GameSettings>>();

            var settings = _settingsRepository.Read();
            var device = container.GetService<GraphicsDevice>();
            var content = container.GetService<ContentManager>();

            _view = device.Viewport;
            _painter = container.GetService<IPainter>();
            _font = content.Load<SpriteFont>("Fonts/simxel.font");
            
            _draws.Add(DrawLifes);

            if (settings.ToggleFramerate.Toggle)
            {
                _draws.Add(DrawFrameRate);
            }

            Lifes = 3;
        }

        public int Lifes { get; set; }

        public void Draw(float time)
        {
            foreach (var draw in _draws) draw(time);
        }

        private void DrawLifes(float time)
        {
            var text = $"HP: {Lifes}";
            var size = _font.MeasureString(text);
            var position = new Vector2(0, size.Y); // TODO: Fix the bug. When I set Y = 0 the simxel font partialy out of screen!

            _painter.DrawString(_font, text, position, Color.White);
        }

        private void DrawFrameRate(float time)
        {
            var rate = $"{Math.Round(1 / time)}fps";
            var size = _font.MeasureString(rate);
            var position = new Vector2(_view.Width - size.X, size.Y); // TODO: Fix the bug. When I set Y = 0 the simxel font partialy out of screen!

            _painter.DrawString(_font, rate, position, Color.White);
        }
    }
}
