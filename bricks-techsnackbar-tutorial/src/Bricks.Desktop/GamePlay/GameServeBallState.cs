using Bricks.Desktop.Engine.StateManagement;
using Bricks.Desktop.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bricks.Desktop.GamePlay
{
    public sealed class GameServeBallState : State<GamePlayContext>
    {
        private const string StartMsg = "Press <Space> or Click Mouse to Start";

        public GameServeBallState(GamePlayContext context)
            : base(context)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Context.SpriteBatch.Begin();

            Vector2 startSpace = Context.LabelFont.MeasureString(StartMsg);
            Context.SpriteBatch.DrawString(Context.LabelFont, StartMsg, new Vector2((Context.ScreenWidth - startSpace.X) / 2, Context.ScreenHeight / 2), Color.White);

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
                float ballX = Context.Paddle.Position.X + (Context.Paddle.Width) / 2;
                float ballY = Context.Paddle.Position.Y - Context.Ball.Height;

                Context.Ball.Position = new Vector2(ballX, ballY);
                Context.Ball.Velocity = new Vector2(-3, -3);
                
                const float volume = 1;
                const float pitch = 0.0f;
                const float pan = 0.0f;
                Context.StartSound.Play(volume, pitch, pan);

                Context.Entities.Add(Context.Ball);

                OnStateUpdate(new GamePlayState(Context));
            }

            Context.OldMouseState = newMouseState;
            Context.OldKeyboardState = newKeyboardState;
        }
    }
}