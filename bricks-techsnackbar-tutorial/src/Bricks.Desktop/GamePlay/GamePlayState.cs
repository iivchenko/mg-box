using Bricks.Desktop.Engine.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bricks.Desktop.GamePlay
{
    public sealed class GamePlayState : State<GamePlayContext>
    {
        public GamePlayState(GamePlayContext context)
            : base(context)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Context.Graphics.Clear(Color.Black);

            Context.SpriteBatch.Begin();

            foreach (var entity in Context.Entities) entity.Draw(gameTime);

            string scoreMsg = "Score: " + Context.Ball.Score.ToString("00000");
            Vector2 space = Context.LabelFont.MeasureString(scoreMsg);
            Context.SpriteBatch.DrawString(Context.LabelFont, scoreMsg, new Vector2((Context.ScreenWidth - space.X) / 2, Context.ScreenHeight - 40), Color.White);
            Context.SpriteBatch.DrawString(Context.LabelFont, Context.BallsRemaining.ToString(), new Vector2(40, 10), Color.White);

            Context.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (Context.Ball.bricksCleared >= 70)
            {
                OnStateUpdate(new GameInitializeState(Context));
                return;
            }

            bool inPlay = Context.Ball.Move(Context.Wall, Context.Entities, Context.Paddle);
            if (!inPlay)
            {
                Context.BallsRemaining--;
                var state = Context.BallsRemaining < 1
                    ? new GameOverState(Context) as IState<GamePlayContext>
                    : new GameServeBallState(Context) as IState<GamePlayContext>;

                OnStateUpdate(state);
            }

            var newKeyboardState = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            if (Context.OldMouseState.X != newMouseState.X)
            {
                if (newMouseState.X >= 0 || newMouseState.X < Context.ScreenWidth)
                {
                    Context.Paddle.MoveTo(newMouseState.X);
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                Context.Paddle.MoveLeft();
            }
            if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                Context.Paddle.MoveRight();
            }

            Context.OldMouseState = newMouseState;
            Context.OldKeyboardState = newKeyboardState;
        }
    }
}