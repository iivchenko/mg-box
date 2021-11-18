using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Numerics;

using XGameTime = Microsoft.Xna.Framework.GameTime;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class StarScreen : GameScreen
    {
        private SpriteSheet _spriteSheet;
        private IViewport _viewport;
        private IPainter _painter;

        private List<Star> _stars;
        private Random _random;

        public StarScreen()
            : base()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Initialize()
        {
            _viewport = ScreenManager.Container.GetService<IViewport>();
            _painter = ScreenManager.Container.GetService<IPainter>();

            var content = ScreenManager.Container.GetService<IContentProvider>();

            _spriteSheet = content.Load<SpriteSheet>("SpriteSheets/Asteroids.sheet");

            _stars = new List<Star>();
            _random = new Random();

            var block = 96;
            var sprite = _spriteSheet["star1"];
            var origin = new Vector2(sprite.Width / 2.0f, sprite.Height / 2.0f);
            for (var x = 0; x <= _viewport.Width / block; x++)
                for (var y = 0; y <= _viewport.Height / block; y++)
                {
                    if (_random.Next(2) == 0)
                        continue;
                    var scale = _random.Next(30, 85) / 100.0f;
                    var star = new Star
                    {
                        Color = new Color((byte)_random.Next(255), (byte)_random.Next(255), (byte)_random.Next(255), 255) * (_random.Next(35, 70)/100.0f),
                        ColorConst = _random.Next(int.MaxValue),
                        Origin = origin,
                        Position = new Vector2(_random.Next(block) + x * block, _random.Next(block) + y * block),
                        Rotation = _random.Next(0, 366).AsRadians(),
                        Scale = new Vector2(scale, scale),
                        Sprite = sprite
                    };

                    _stars.Add(star);
                }
        }

        public override void Update(XGameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(XGameTime gameTime)
        {
            _stars.Iter(star =>
                {
                    var color = star.Color * (float)Math.Abs(Math.Sin(gameTime.TotalGameTime.TotalSeconds + star.ColorConst) * 0.75);
                    _painter.Draw(star.Sprite, star.Position, star.Origin, star.Scale, star.Rotation, color);
                });
        }

        private class Star
        {
            public Sprite Sprite { get; set; }

            public Vector2 Origin { get; set; }

            public float ColorConst { get; set; }

            public Color Color { get; set; }

            public Vector2 Scale { get; set; }

            public float Rotation { get; set; }

            public Vector2 Position { get; set; }
        }
    }
}
