using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bricks.Desktop
{
    public class BricksGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameContent _gameContent;
        private GameBorder _gameBorder;
        private Ball _ball;
        private Ball _staticBall; //used to draw image next to remaining ball count at top of screen

        private Paddle _paddle;
        private Wall _wall;
        private int _screenWidth = 0;
        private int _screenHeight = 0;
        private MouseState _oldMouseState;
        private KeyboardState _oldKeyboardState;
        private bool readyToServeBall = true;
        private int ballsRemaining = 3;

        public BricksGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        private void ServeBall()
        {
            if (ballsRemaining < 1)
            {
                ballsRemaining = 3;
                _ball.Score = 0;
                _wall = new Wall(1, 50, _spriteBatch, _gameContent);
            }
            readyToServeBall = false;
            float ballX = _paddle.X + (_paddle.Width) / 2;
            float ballY = _paddle.Y - _ball.Height;
            _ball.Launch(ballX, ballY, -3, -3);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _gameContent = new GameContent(Content);

            _screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //set game to 502x700 or screen max if smaller
            if (_screenWidth >= 502)
            {
                _screenWidth = 502;
            }
            if (_screenHeight >= 700)
            {
                _screenHeight = 700;
            }
            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            _graphics.ApplyChanges();

            //create game objects
            int paddleX = (_screenWidth - _gameContent.ImgPaddle.Width) / 2;
            //we'll center the paddle on the screen to start
            int paddleY = _screenHeight - 100;  //paddle will be 100 pixels from the bottom of the screen
            _paddle = new Paddle(paddleX, paddleY, _screenWidth, _spriteBatch, _gameContent);  // create the game paddle
            _wall = new Wall(1, 50, _spriteBatch, _gameContent);
            _gameBorder = new GameBorder(_screenWidth, _screenHeight, _spriteBatch, _gameContent);
            _ball = new Ball(_screenWidth, _screenHeight, _spriteBatch, _gameContent);

            _staticBall = new Ball(_screenWidth, _screenHeight, _spriteBatch, _gameContent);
            _staticBall.X = 25;
            _staticBall.Y = 25;
            _staticBall.Visible = true;
            _staticBall.UseRotation = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;  //our window is not active don't update
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var newKeyboardState = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            //process mouse move
            if (_oldMouseState.X != newMouseState.X)
            {
                if (newMouseState.X >= 0 || newMouseState.X < _screenWidth)
                {
                    _paddle.MoveTo(newMouseState.X);
                }
            }

            //process left-click
            if (newMouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.X == newMouseState.X && _oldMouseState.Y == newMouseState.Y && readyToServeBall)
            {
                ServeBall();
            }

            //process keyboard events
            if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                _paddle.MoveLeft();
            }
            if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                _paddle.MoveRight();
            }
            if (_oldKeyboardState.IsKeyUp(Keys.Space) && newKeyboardState.IsKeyDown(Keys.Space) && readyToServeBall)
            {
                ServeBall();
            }

            _oldMouseState = newMouseState; // this saves the old state
            _oldKeyboardState = newKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _paddle.Draw();
            _wall.Draw();
            _gameBorder.Draw();

            if (_ball.Visible)
            {
                bool inPlay = _ball.Move(_wall, _paddle);
                if (inPlay)
                {
                    _ball.Draw();
                }
                else
                {
                    ballsRemaining--;
                    readyToServeBall = true;
                }
            }

            _staticBall.Draw();

            string scoreMsg = "Score: " + _ball.Score.ToString("00000");
            Vector2 space = _gameContent.LabelFont.MeasureString(scoreMsg);
            _spriteBatch.DrawString(_gameContent.LabelFont, scoreMsg, new Vector2((_screenWidth - space.X) / 2, _screenHeight - 40), Color.White);
            if (_ball.bricksCleared >= 70)
            {
                _ball.Visible = false;
                _ball.bricksCleared = 0;
                _wall = new Wall(1, 50, _spriteBatch, _gameContent);
                readyToServeBall = true;
            }
            if (readyToServeBall)
            {
                if (ballsRemaining > 0)
                {
                    string startMsg = "Press <Space> or Click Mouse to Start";
                    Vector2 startSpace = _gameContent.LabelFont.MeasureString(startMsg);
                    _spriteBatch.DrawString(_gameContent.LabelFont, startMsg, new Vector2((_screenWidth - startSpace.X) / 2, _screenHeight / 2), Color.White);
                }
                else
                {
                    string endMsg = "Game Over";
                    Vector2 endSpace = _gameContent.LabelFont.MeasureString(endMsg);
                    _spriteBatch.DrawString(_gameContent.LabelFont, endMsg, new Vector2((_screenWidth - endSpace.X) / 2, _screenHeight / 2), Color.White);
                }
            }
            _spriteBatch.DrawString(_gameContent.LabelFont, ballsRemaining.ToString(), new Vector2(40, 10), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
