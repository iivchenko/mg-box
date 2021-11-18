using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayHud : IEntity, IDrawable
    {
        private readonly IOptionsMonitor<GameSettings> _settings;

        private readonly IPainter _painter;
        private readonly Font _font;
        private readonly IViewport _view;
        private readonly IFontService _fontService;
        private readonly IList<Action<float>> _draws;

        public GamePlayHud(
            IOptionsMonitor<GameSettings> settings,
            IViewport viewport,
            IPainter painter,
            IContentProvider content,
            IFontService fontService)
        {
            _draws = new List<Action<float>>();

            _settings = settings;
            _view = viewport;
            _painter = painter;
            _fontService = fontService;

            _font = content.Load<Font>("Fonts/kenney-future.h4.font");

            _draws.Add(DrawLifes);

            if (_settings.CurrentValue.ToggleFramerate.Toggle)
            {
                _draws.Add(DrawFrameRate);
            }

            Lifes = 3;

            StartTime = DateTime.Now;
        }

        public IEnumerable<string> Tags => Enumerable.Empty<string>();

        public int Scores { get; set; }

        public int Lifes { get; set; }

        public DateTime StartTime { get; private set; }

        public void Draw(float time)
        {
            foreach (var draw in _draws) draw(time);
        }

        private void DrawLifes(float time)
        {
            var text = $"HP: {Lifes}\nScores: {Scores}";
            var position = new Vector2(0, 0); 

            _painter.DrawString(_font, text, position, Colors.White);
        }

        private void DrawFrameRate(float time)
        {
            var rate = $"{Math.Round(1 / time)}fps";
            var size = _fontService.MeasureText(rate, _font);
            var position = new Vector2(_view.Width - size.Width, size.Height); 

            _painter.DrawString(_font, rate, position, Colors.White);
        }
    }
}
