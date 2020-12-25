using Bricks.Desktop.Engine.StateManagement;
using Bricks.Desktop.GamePlay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bricks.Desktop
{
    public class BricksGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private IState<GamePlayContext> _state;

        public BricksGame()
        {
            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            var context = new GamePlayContext
            {
                Content = Content,
                Graphics = _graphics.GraphicsDevice,
                SpriteBatch = new SpriteBatch(GraphicsDevice),
                ScreenWidth = 502,
                ScreenHeight = 700
            };

            _graphics.PreferredBackBufferWidth = context.ScreenWidth;
            _graphics.PreferredBackBufferHeight = context.ScreenHeight;
            _graphics.ApplyChanges();

            _state = new GameInitializeState(context);

            _state.StateUpdate += OnStateChange;
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                _state.Update(gameTime);

                base.Update(gameTime);
            }           
        }

        protected override void Draw(GameTime gameTime)
        {
            _state.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void OnStateChange(object sender, IState<GamePlayContext> args)
        {
            _state.StateUpdate -= OnStateChange;

            _state = args;

            _state.StateUpdate += OnStateChange;
        }
    }
}
