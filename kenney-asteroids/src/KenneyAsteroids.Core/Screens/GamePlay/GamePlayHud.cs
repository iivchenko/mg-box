using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayHud : IEntity, IDrawable
    {
        private readonly IOptionsMonitor<GameSettings> _settings;

        private readonly IPainter _painter;
        private readonly SpriteFont _font;
        private readonly IViewport _view;
        private readonly IList<Action<float>> _draws;

        public GamePlayHud(
            IOptionsMonitor<GameSettings> settings,
            IViewport viewport,
            IPainter painter,
            IContentProvider content)
        {
            _draws = new List<Action<float>>();

            _settings = settings;
            _view = viewport;
            _painter = painter;

            _font = content.Load<SpriteFont>("Fonts/kenney-future.h4.font");
        }

        public int Scores { get; set; }

        public int Lifes { get; set; }

        public DateTime StartTime { get; private set; }

        public void Initialize()
        {
            _draws.Clear();
            _draws.Add(DrawLifes);

            if (_settings.CurrentValue.ToggleFramerate.Toggle)
            {
                _draws.Add(DrawFrameRate);
            }

            Lifes = 3;

            StartTime = DateTime.Now;
        }

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
            var size = _font.MeasureString(rate);
            var position = new Vector2(_view.Width - size.X, size.Y); // TODO: Fix the bug. When I set Y = 0 the simxel font partialy out of screen!

            _painter.DrawString(_font, rate, position, Colors.White);
        }
    }
}
