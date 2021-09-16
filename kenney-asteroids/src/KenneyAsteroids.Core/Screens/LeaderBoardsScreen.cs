﻿using KenneyAsteroids.Core.Leaderboards;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Content;
using KenneyAsteroids.Engine.Graphics;
using KenneyAsteroids.Engine.Screens;
using KenneyAsteroids.Engine.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Globalization;
using System.Linq;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class LeaderBoardsScreen : GameScreen
    {
        private IPainter _painter;
        private IViewport _viewport;

        private SpriteFont _h1;
        private SpriteFont _h2;
        private SpriteFont _h4;

        private LeaderboardsManager _magager;

        private Control _root;

        public override void Initialize()
        {
            base.Initialize();

            _painter = ScreenManager.Container.GetService<IPainter>();
            _viewport = ScreenManager.Container.GetService<IViewport>();
            _magager = ScreenManager.Container.GetService<LeaderboardsManager>();

            var content = ScreenManager.Container.GetService<IContentProvider>();
            _h1 = content.Load<SpriteFont>("Fonts/kenney-future.h1.font");
            _h2 = content.Load<SpriteFont>("Fonts/kenney-future.h2.font");
            _h4 = content.Load<SpriteFont>("Fonts/kenney-future.h4.font");

            _root = new ScrollingPanelControl();
            _root.AddChild(CreateTitleControl());
            _root.AddChild(CreateBoard());
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _root.Draw(new DrawContext
            {
                Viewport = _viewport,
                Painter = _painter,
                DrawOffset = _root.Position,
                GameTime = gameTime
            });
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _root.Update(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            if (
                input.IsNewKeyPress(Keys.Escape, null, out _) || 
                input.IsNewButtonPress(Buttons.B, null, out _))
            {
                ExitScreen();
            }
        }

        private Control CreateTitleControl()
        {
            var text = "Leaderboard";
            var x = _viewport.Width / 2 - _h1.MeasureString(text).X / 2;
            return new TextControl(text, _h1, Colors.Blue, new Vector2(x, 30));
        }

        private Control CreateBoard()
        {
            const int colWidth = 800;
            PanelControl newList = new PanelControl();

            var header = new PanelControl();
            header.AddChild(new TextControl("Name", _h2, Colors.Turquoise, new Vector2(colWidth * 0, 0)));
            header.AddChild(new TextControl("Score", _h2, Colors.Turquoise, new Vector2(colWidth * 1, 0)));
            header.AddChild(new TextControl("Time", _h2, Colors.Turquoise, new Vector2(colWidth * 2, 0)));
            header.AddChild(new TextControl("Date", _h2, Colors.Turquoise, new Vector2(colWidth * 3, 0)));
            newList.AddChild(header);

            _magager
                .GetLeaders()
                .Select(x =>
                {
                    var textColor = Colors.White;

                    var panel = new PanelControl();

                    // Player name
                    panel.AddChild(
                        new TextControl
                        {
                            Text = x.Name,
                            Font = _h4,
                            Color = textColor,
                            Position = new Vector2(colWidth * 0, 0)
                        });

                    // Score
                    panel.AddChild(
                        new TextControl
                        {
                            Text = x.Score.ToString(),
                            Font = _h4,
                            Color = textColor,
                            Position = new Vector2(colWidth * 1, 0)
                        });

                    // Time
                    panel.AddChild(
                        new TextControl
                        {
                            Text = x.PlayedTime.ToString("hh\\:mm\\:ss"),
                            Font = _h4,
                            Color = textColor,
                            Position = new Vector2(colWidth * 2, 0)
                        });

                    // Date
                    panel.AddChild(
                        new TextControl
                        {
                            Text = x.ScoreDate.ToString("dd-MM-yyyy hh\\:mm", CultureInfo.InvariantCulture),
                            Font = _h4,
                            Color = textColor,
                            Position = new Vector2(colWidth * 3, 0)
                        });

                    return panel;
                })
                .Iter(newList.AddChild);

            newList.LayoutColumn(0, 100, 0);

            newList.Position = new Vector2(_viewport.Width / 2 - newList.Size.X / 2, 120);

            return newList;
        }  
    }
}
