using Bricks.Desktop.Engine.StateManagement;
using Bricks.Desktop.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Bricks.Desktop.GamePlay
{
    public sealed class GamePlayState : State<GamePlayContext>
    {
        public GamePlayState(GamePlayContext context)
            : base(context)
        {
            foreach (var brick in Context.Wall)
            {
                brick.Scored +=
                    (b, score) =>
                    {
                        Context.Hud.UpdateScore(score);
                        Context.Wall.Remove(brick);
                        Context.Entities.Remove(brick);
                        Context.World.Remove(brick);
                    };
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Context.Graphics.Clear(Color.Black);

            Context.SpriteBatch.Begin();

            foreach (var entity in Context.Entities) entity.Draw(gameTime);

            Context.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Context.Entities.Any(x => x is Brick))
            {
                OnStateUpdate(new GameInitializeState(Context));
                return;
            }

            if (Context.Ball.Position.X < 1)
            {
                Context.Ball.Position = new Vector2(1, Context.Ball.Position.Y);
                Context.Ball.Velocity = new Vector2(Context.Ball.Velocity.X * -1, Context.Ball.Velocity.Y);
                Context.WallBounceSound.Play();
            }
            else if (Context.Ball.Position.X > Context.ScreenWidth - Context.Ball.Width + 5)
            {
                Context.Ball.Position = new Vector2(Context.ScreenWidth - Context.Ball.Width + 5, Context.Ball.Position.Y);
                Context.Ball.Velocity = new Vector2(Context.Ball.Velocity.X * -1, Context.Ball.Velocity.Y);
                Context.WallBounceSound.Play();
            }
            else if (Context.Ball.Position.Y < 1)
            {
                Context.Ball.Position = new Vector2(Context.Ball.Position.Y, 1);
                Context.Ball.Velocity = new Vector2(Context.Ball.Velocity.X, Context.Ball.Velocity.Y * -1);
                Context.WallBounceSound.Play();
            }
            else if (Context.Ball.Position.Y > Context.ScreenHeight)
            {
                Context.MissSound.Play();

                Context.BallsRemaining--;
                Context.Hud.BallsRemaining--;
                var state = Context.BallsRemaining < 1
                    ? new GameOverState(Context) as IState<GamePlayContext>
                    : new GameServeBallState(Context) as IState<GamePlayContext>;

                OnStateUpdate(state);
                return;
            }

            var newKeyboardState = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            if (Context.OldMouseState.X != newMouseState.X)
            {
                if (newMouseState.X >= 0 || newMouseState.X < Context.ScreenWidth)
                {
                    Context.Paddle.MoveTo(newMouseState.X);

                    if (Context.Paddle.Position.X < 1)
                    {
                        Context.Paddle.Position = new Vector2(1, Context.Paddle.Position.Y);
                    }
                    else if ((Context.Paddle.Position.X + Context.Paddle.Width) > Context.ScreenWidth)
                    {
                        Context.Paddle.Position = new Vector2(Context.ScreenWidth - Context.Paddle.Width, Context.Paddle.Position.Y);
                    }
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                Context.Paddle.MoveLeft();

                if (Context.Paddle.Position.X < 1)
                {
                    Context.Paddle.Position = new Vector2(1, Context.Paddle.Position.Y);
                }
            }
            if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                Context.Paddle.MoveRight();

                if ((Context.Paddle.Position.X + Context.Paddle.Width) > Context.ScreenWidth)
                {
                    Context.Paddle.Position = new Vector2(Context.ScreenWidth - Context.Paddle.Width, Context.Paddle.Position.Y);
                }
            }

            foreach (var entity in Context.Entities.ToList()) entity.Update(gameTime);

            Context.OldMouseState = newMouseState;
            Context.OldKeyboardState = newKeyboardState;
        }
    }
}