using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KenneyAsteroids.Engine.Graphics;

namespace KenneyAsteroids.Engine.Screens
{
    public sealed class MessageBoxScreen : GameScreen
    {
        private readonly string _message;

        private Sprite _gradient;
        private SpriteFont _font;

        public MessageBoxScreen(string message, IServiceProvider container)
            : base(container)
        {
            _message = message;

            IsPopup = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        public override void Initialize()
        {
            base.Initialize();

            _gradient = new Sprite(Content.Load<Texture2D>("Sprites/gradient.sprite")); // TODO: Make Sprite pipeline loader
            _font = Content.Load<SpriteFont>("Fonts/simxel.font");
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
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
        }

        public override void Draw(float time)
        {
            base.Draw(time);

            // Darken down any other screens that were drawn beneath the popup.
            FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            var viewport = GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            var textSize = _font.MeasureString(_message);
            var textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            var color = Color.White * TransitionAlpha;

            // Draw the background rectangle.
            DrawSystem.Draw(_gradient, backgroundRectangle, color);

            // Draw the message box text.
            DrawSystem.DrawString(_font, _message, textPosition, color);
        }
    }
}
