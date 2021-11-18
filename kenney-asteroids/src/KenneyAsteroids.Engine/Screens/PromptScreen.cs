using System;
using System.Numerics;
using KenneyAsteroids.Engine.Graphics;
using Microsoft.Extensions.DependencyInjection;
using KenneyAsteroids.Engine.UI;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using KenneyAsteroids.Engine.Content;

using XGameTime = Microsoft.Xna.Framework.GameTime;
using XPlayerIndex = Microsoft.Xna.Framework.PlayerIndex;


namespace KenneyAsteroids.Engine.Screens
{
    public sealed class PromptScreen : GameScreen
    {
        private readonly string _message;
        private Sprite _gradientSprite;
        private TextControl _text;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        public PromptScreen(string message)
        {
            _message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public string Text { get => _text.Text; }

        public override void Initialize()
        {
            var content = ScreenManager.Container.GetService<IContentProvider>();
            var fontService = ScreenManager.Container.GetService<IFontService>();

            _gradientSprite = content.Load<Sprite>("Sprites/gradient.sprite");
            _text = new TextControl(string.Empty, ScreenManager.Font, fontService, Colors.Yellow);
        }

        public override void HandleInput(InputState input)
        {
            XPlayerIndex playerIndex;

            if (input.IsNewKeyPress(Keys.Space, null, out _))
            {
                _text.Text += " ";
            }
            else if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));

                ExitScreen();
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));

                ExitScreen();
            }
            else if (input.IsNewKeyPress(Keys.Back, null, out _) && _text.Text.Length > 0)
            {
                _text.Text = _text.Text.Substring(0, _text.Text.Length - 1);
            }           
            else
            {
                var key =
                    input
                        .CurrentKeyboardStates
                        .First()
                        .GetPressedKeys()
                        .Where(x => input.IsNewKeyPress(x, null, out _))
                        .Where(x => (int)x >= 65 && (int)x <= 90)
                        .FirstOrDefault();

                if  (key != Keys.None && _text.Text.Length < 15)
                {
                    _text.Text += key.ToString();
                }
            }
        }

        public override void Draw(XGameTime gameTime)
        {
            var painter = ScreenManager.Painter;
            var content = ScreenManager.Container.GetService<IContentProvider>();
            var fontService = ScreenManager.Container.GetService<IFontService>();
            var font = content.Load<Font>("Fonts/kenney-future.h3.font");

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            var viewport = ScreenManager.Container.GetService<IViewport>();
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            var size = fontService.MeasureText(_message, font);
            var textSize = new Vector2(size.Width, size.Height);
            var textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = Colors.White * TransitionAlpha;
            // Draw the background rectangle.
            painter.Draw(_gradientSprite, backgroundRectangle, color);

            // Draw the message box text.
            painter.DrawString(font, _message, textPosition, color);

            _text.Draw(new DrawContext
            {
                Painter = painter,
                Viewport = viewport,
                DrawOffset = (viewportSize - textSize) / 2 + new Vector2(0, textSize.Y)
            });
        }
    }
}
