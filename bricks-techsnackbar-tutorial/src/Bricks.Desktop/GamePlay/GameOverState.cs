using Bricks.Desktop.Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bricks.Desktop.GamePlay
{
    public sealed class GameOverState : State<GamePlayContext>
    {
        private const string GameOverMessage = "Game Over";

        public GameOverState(GamePlayContext context)
            : base(context)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Context.Graphics.Clear(Color.Black);

            Context.SpriteBatch.Begin();

            var endSpace = Context.LabelFont.MeasureString(GameOverMessage);
            Context.SpriteBatch.DrawString(
                Context.LabelFont,
                GameOverMessage,
                new Vector2((Context.ScreenWidth - endSpace.X) / 2, Context.ScreenHeight / 2),
                Color.White);

            Context.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            var newKeyboardState = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            if (
                newMouseState.LeftButton == ButtonState.Released &&
                Context.OldMouseState.LeftButton == ButtonState.Pressed &&
                Context.OldMouseState.X == newMouseState.X &&
                Context.OldMouseState.Y == newMouseState.Y)
            {
                OnStateUpdate(new GameInitializeState(Context));
            }

            Context.OldMouseState = newMouseState;
            Context.OldKeyboardState = newKeyboardState;
        }
    }
}
