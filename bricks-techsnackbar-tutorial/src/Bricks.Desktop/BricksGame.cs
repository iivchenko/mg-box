using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bricks.Desktop
{
    public class BricksGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
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

        // Content
        private Texture2D _piexel;
        private Texture2D _brickSprite;
        private Texture2D _paddleSprite;
        private Texture2D _ballSprite;

        private SoundEffect _startSound;
        private SoundEffect _brickSound;
        private SoundEffect _paddleBounceSound;
        private SoundEffect _wallBounceSound;
        private SoundEffect _missSound; // TODO: USE IT!

        private SpriteFont _labelFont;

        public BricksGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

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
            int paddleX = (_screenWidth - _paddleSprite.Width) / 2;
            //we'll center the paddle on the screen to start
            int paddleY = _screenHeight - 100;  //paddle will be 100 pixels from the bottom of the screen
            _paddle = new Paddle(paddleX, paddleY, _screenWidth, _spriteBatch, _paddleSprite);  // create the game paddle
            _wall = new Wall(1, 50, _spriteBatch, _brickSprite);
            _gameBorder = new GameBorder(_screenWidth, _screenHeight, _spriteBatch, _piexel);

            _ball =
                new Ball(
                    _screenWidth,
                    _screenHeight,
                    _spriteBatch,
                    _ballSprite,
                    _startSound,
                    _wallBounceSound,
                    _paddleBounceSound,
                    _brickSound);

            _staticBall =
                new Ball(
                    _screenWidth,
                    _screenHeight,
                    _spriteBatch,
                    _ballSprite,
                    null,
                    null,
                    null,
                    null)
                {
                    X = 25,
                    Y = 25,
                    Visible = true,
                    UseRotation = false
                };
        }

        protected override void LoadContent()
        {
            _piexel = Content.Load<Texture2D>("Images/Pixel");
            _brickSprite = Content.Load<Texture2D>("Images/Brick");
            _paddleSprite = Content.Load<Texture2D>("Images/Paddle");
            _ballSprite = Content.Load<Texture2D>("Images/Ball");

            _startSound = Content.Load<SoundEffect>("Sounds/Start");
            _brickSound = Content.Load<SoundEffect>("Sounds/Brick");
            _paddleBounceSound = Content.Load<SoundEffect>("Sounds/PaddleBounce");
            _wallBounceSound = Content.Load<SoundEffect>("Sounds/WallBounce");
            _missSound = Content.Load<SoundEffect>("Sounds/Miss");

            _labelFont = Content.Load<SpriteFont>("Fonts/Arial20");
        }

        private void ServeBall()
        {
            if (ballsRemaining < 1)
            {
                ballsRemaining = 3;
                _ball.Score = 0;
                _wall = new Wall(1, 50, _spriteBatch, _brickSprite);
            }
            readyToServeBall = false;
            float ballX = _paddle.X + (_paddle.Width) / 2;
            float ballY = _paddle.Y - _ball.Height;
            _ball.Launch(ballX, ballY, -3, -3);
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
            Vector2 space = _labelFont.MeasureString(scoreMsg);
            _spriteBatch.DrawString(_labelFont, scoreMsg, new Vector2((_screenWidth - space.X) / 2, _screenHeight - 40), Color.White);
            if (_ball.bricksCleared >= 70)
            {
                _ball.Visible = false;
                _ball.bricksCleared = 0;
                _wall = new Wall(1, 50, _spriteBatch, _brickSprite);
                readyToServeBall = true;
            }
            if (readyToServeBall)
            {
                if (ballsRemaining > 0)
                {
                    string startMsg = "Press <Space> or Click Mouse to Start";
                    Vector2 startSpace = _labelFont.MeasureString(startMsg);
                    _spriteBatch.DrawString(_labelFont, startMsg, new Vector2((_screenWidth - startSpace.X) / 2, _screenHeight / 2), Color.White);
                }
                else
                {
                    string endMsg = "Game Over";
                    Vector2 endSpace = _labelFont.MeasureString(endMsg);
                    _spriteBatch.DrawString(_labelFont, endMsg, new Vector2((_screenWidth - endSpace.X) / 2, _screenHeight / 2), Color.White);
                }
            }
            _spriteBatch.DrawString(_labelFont, ballsRemaining.ToString(), new Vector2(40, 10), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
