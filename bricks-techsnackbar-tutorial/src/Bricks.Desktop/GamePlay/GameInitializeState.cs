using Bricks.Desktop.Engine;
using Bricks.Desktop.Engine.StateManagement;
using Bricks.Desktop.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Bricks.Desktop.GamePlay
{
    public sealed class GameInitializeState : State<GamePlayContext>
    {
        public GameInitializeState(GamePlayContext context)
            : base(context)
        {
        }

        public override void Draw(GameTime time)
        {
        }

        public override void Update(GameTime gameTime)
        {
            LoadContent();

            Context.Paddle = new Paddle(
            new Vector2(
                (Context.ScreenWidth - Context.PaddleSprite.Width) / 2,
                Context.ScreenHeight - 100),
            Context.SpriteBatch,
            Context.PaddleSprite);

            Context.Wall = WallFactory.CreateWall(1, 50, Context.BrickSprite, Context.SpriteBatch);
            Context.GameBorder = new Border(Context.ScreenWidth, Context.ScreenHeight, Context.SpriteBatch, Context.Piexel);

            Context.Ball =
                new Ball(
                    Vector2.Zero,
                    Context.ScreenWidth,
                    Context.ScreenHeight,
                    Context.SpriteBatch,
                    Context.BallSprite,
                    Context.WallBounceSound,
                    Context.PaddleBounceSound,
                    Context.BrickSound);

            Context.Entities = new List<IEntity>
                {
                    Context.Paddle,
                    Context.Ball,
                    Context.GameBorder
                };

            Context.Entities.AddRange(Context.Wall);

            Context.BallsRemaining = 3;

            Context.Hud = new Hud(
                Context.LabelFont, 
                Context.ScreenHeight, 
                Context.ScreenWidth, 
                Context.Ball, 
                Context.BallSprite,
                Context.SpriteBatch)
            {
                BallsRemaining = Context.BallsRemaining = 3
            };

            Context.Entities.Add(Context.Hud);
            

            OnStateUpdate(new GameServeBallState(Context));
        }

        private void LoadContent()
        {
            Context.Piexel = Context.Content.Load<Texture2D>("Images/Pixel");
            Context.BrickSprite = Context.Content.Load<Texture2D>("Images/Brick");
            Context.PaddleSprite = Context.Content.Load<Texture2D>("Images/Paddle");
            Context.BallSprite = Context.Content.Load<Texture2D>("Images/Ball");

            Context.StartSound = Context.Content.Load<SoundEffect>("Sounds/Start");
            Context.BrickSound = Context.Content.Load<SoundEffect>("Sounds/Brick");
            Context.PaddleBounceSound = Context.Content.Load<SoundEffect>("Sounds/PaddleBounce");
            Context.WallBounceSound = Context.Content.Load<SoundEffect>("Sounds/WallBounce");
            Context.MissSound = Context.Content.Load<SoundEffect>("Sounds/Miss");

            Context.LabelFont = Context.Content.Load<SpriteFont>("Fonts/Arial20");
        }
    }
}