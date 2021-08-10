using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayHud : IEntity, Engine.IDrawable
    {
        private readonly IOptionsMonitor<GameSettings> _settings;

        private readonly IPainter _painter;
        private readonly SpriteFont _font;
        private readonly IViewport _view;
        private readonly IList<Action<float>> _draws;

        public GamePlayHud(IServiceProvider container)
        {
            _draws = new List<Action<float>>();

            _settings = container.GetService<IOptionsMonitor<GameSettings>>();
            _view = container.GetService<IViewport>();

            var content = container.GetService<ContentManager>();

            _painter = container.GetService<IPainter>();
            _font = content.Load<SpriteFont>("Fonts/simxel.font");
            
            _draws.Add(DrawLifes);

            if (_settings.CurrentValue.ToggleFramerate.Toggle)
            {
                _draws.Add(DrawFrameRate);
            }

            Lifes = 3;
        }

        public int Scores { get; set; }

        public int Lifes { get; set; }

        public void Draw(float time)
        {
            foreach (var draw in _draws) draw(time);
        }

        private void DrawLifes(float time)
        {
            var text = $"HP: {Lifes}\nScores: {Scores}";
            var size = _font.MeasureString(text);
            var position = new Vector2(0, size.Y); // TODO: Fix the bug. When I set Y = 0 the simxel font partialy out of screen!

            _painter.DrawString(_font, text, position, Colors.White);
        }

        private void DrawFrameRate(float time)
        {
            var rate = $"{Math.Round(1 / time)}fps";
            var size = _font.MeasureString(rate);
            var position = new Vector2(_view.Width - size.X, size.Y); // TODO: Fix the bug. When I set Y = 0 the simxel font partialy out of screen!

            _painter.DrawString(_font, rate, position, Colors.White);
        }
    }
}
