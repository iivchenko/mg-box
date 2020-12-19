using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Bricks.Desktop
{
    public class BricksGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;

        private GameBorder _gameBorder;
        private Ball _ball;
        private Ball _staticBall;
        private Paddle _paddle;
        private List<Brick> _wall;

        private int _screenWidth = 0;
        private int _screenHeight = 0;
        private MouseState _oldMouseState;
        private KeyboardState _oldKeyboardState;
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

        private IGameState _state;

        public BricksGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _state = new InitializeNewGameState(this);
        }

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";

            base.Initialize();

            IsMouseVisible = true;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _graphics.PreferredBackBufferWidth = _screenWidth = 502;
            _graphics.PreferredBackBufferHeight = _screenHeight = 700;
            _graphics.ApplyChanges();
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

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _state.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _state.Draw(gameTime);

            base.Draw(gameTime);
        }

        private static List<Brick> CreateWall(float x, float y, Texture2D brick, SpriteBatch spriteBatch)
        {
            var wall = new Brick[7, 10];

            for (int i = 0; i < 7; i++)
            {
                var color = i switch
                {
                    0 => Color.Red,
                    1 => Color.Orange,
                    2 => Color.Yellow,
                    3 => Color.Green,
                    4 => Color.Blue,
                    5 => Color.Indigo,
                    6 => Color.Violet,
                    _ => Color.White
                };

                var brickY = y + i * brick.Height;

                for (int j = 0; j < 10; j++)
                {
                    var brickX = x + j * brick.Width;
                    wall[i, j] = new Brick(
                        new Vector2(brickX, brickY), 
                        color, 
                        spriteBatch, 
                        brick,
                        7 - i);
                }
            }

            return wall.OfType<Brick>().ToList();
        }

        private interface IGameState
        {
            void Update(GameTime gameTime);

            void Draw(GameTime gameTime);
        }

        private sealed class InitializeNewGameState : IGameState
        {
            private readonly BricksGame _game;

            public InitializeNewGameState(BricksGame game)
            {
                _game = game;
            }

            public void Draw(GameTime gameTime)
            {
            }

            public void Update(GameTime gameTime)
            {
                _game._paddle = new Paddle(
                new Vector2(
                    (_game._screenWidth - _game._paddleSprite.Width) / 2,
                    _game._screenHeight - 100),
                _game._screenWidth,
                _game._spriteBatch,
                _game._paddleSprite);

                _game._wall = CreateWall(1, 50, _game._brickSprite, _game._spriteBatch);
                _game._gameBorder = new GameBorder(_game._screenWidth, _game._screenHeight, _game._spriteBatch, _game._piexel);

                _game._ball =
                    new Ball(
                        _game._screenWidth,
                        _game._screenHeight,
                        _game._spriteBatch,
                        _game._ballSprite,
                        _game._startSound,
                        _game._wallBounceSound,
                        _game._paddleBounceSound,
                        _game._brickSound);

                _game._staticBall =
                    new Ball(
                        _game._screenWidth,
                        _game._screenHeight,
                        _game._spriteBatch,
                        _game._ballSprite,
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

                _game._state = new GameServeBallState(_game);
            }
        }

        private sealed class GamePlayState : IGameState
        {
            private readonly BricksGame _game;

            public GamePlayState(BricksGame game)
            {
                _game = game;
            }

            public void Draw(GameTime gameTime)
            {
                _game.GraphicsDevice.Clear(Color.Black);

                _game._spriteBatch.Begin();

                _game._paddle.Draw();
                foreach (var brick in _game._wall) brick.Draw();

                _game._gameBorder.Draw();

                _game._ball.Draw();

                _game._staticBall.Draw();

                string scoreMsg = "Score: " + _game._ball.Score.ToString("00000");
                Vector2 space = _game._labelFont.MeasureString(scoreMsg);
                _game._spriteBatch.DrawString(_game._labelFont, scoreMsg, new Vector2((_game._screenWidth - space.X) / 2, _game._screenHeight - 40), Color.White);

                _game._spriteBatch.DrawString(_game._labelFont, _game.ballsRemaining.ToString(), new Vector2(40, 10), Color.White);

                _game._spriteBatch.End();
            }

            public void Update(GameTime gameTime)
            {
                if (_game._ball.bricksCleared >= 70)
                {
                    _game._state = new InitializeNewGameState(_game);
                    return;
                }

                bool inPlay = _game._ball.Move(_game._wall, _game._paddle);
                if (!inPlay)
                {
                    _game.ballsRemaining--;
                    _game._state = _game.ballsRemaining < 1
                        ? new GameOverState(_game) as IGameState
                        : new GameServeBallState(_game) as IGameState;
                }

                var newKeyboardState = Keyboard.GetState();
                var newMouseState = Mouse.GetState();

                if (_game._oldMouseState.X != newMouseState.X)
                {
                    if (newMouseState.X >= 0 || newMouseState.X < _game._screenWidth)
                    {
                        _game._paddle.MoveTo(newMouseState.X);
                    }
                }

                if (newKeyboardState.IsKeyDown(Keys.Left))
                {
                    _game._paddle.MoveLeft();
                }
                if (newKeyboardState.IsKeyDown(Keys.Right))
                {
                    _game._paddle.MoveRight();
                }

                _game._oldMouseState = newMouseState;
                _game._oldKeyboardState = newKeyboardState;
            }
        }

        private sealed class GameServeBallState : IGameState
        {
            private const string StartMsg = "Press <Space> or Click Mouse to Start";
            private readonly BricksGame _game;

            public GameServeBallState(BricksGame game)
            {
                _game = game;
            }

            public void Draw(GameTime gameTime)
            {
                _game._spriteBatch.Begin();

                Vector2 startSpace = _game._labelFont.MeasureString(StartMsg);
                _game._spriteBatch.DrawString(_game._labelFont, StartMsg, new Vector2((_game._screenWidth - startSpace.X) / 2, _game._screenHeight / 2), Color.White);

                _game._spriteBatch.End();
            }

            public void Update(GameTime gameTime)
            {
                var newKeyboardState = Keyboard.GetState();
                var newMouseState = Mouse.GetState();

                if (
                    newMouseState.LeftButton == ButtonState.Released && 
                    _game._oldMouseState.LeftButton == ButtonState.Pressed && 
                    _game._oldMouseState.X == newMouseState.X && 
                    _game._oldMouseState.Y == newMouseState.Y)
                {
                    float ballX = _game._paddle.Position.X + (_game._paddle.Width) / 2;
                    float ballY = _game._paddle.Position.Y - _game._ball.Height;
                    _game._ball.Launch(ballX, ballY, -3, -3);

                    _game._state = new GamePlayState(_game);
                }

                _game._oldMouseState = newMouseState;
                _game._oldKeyboardState = newKeyboardState;
            }
        }

        private sealed class GameOverState : IGameState
        {
            private const string GameOverMessage = "Game Over";
            private readonly BricksGame _game;

            public GameOverState(BricksGame game)
            {
                _game = game;
            }

            public void Draw(GameTime gameTime)
            {
                _game.GraphicsDevice.Clear(Color.Black);

                _game._spriteBatch.Begin();

                var endSpace = _game._labelFont.MeasureString(GameOverMessage);
                _game._spriteBatch.DrawString(
                    _game._labelFont,
                    GameOverMessage,
                    new Vector2((_game._screenWidth - endSpace.X) / 2, _game._screenHeight / 2),
                    Color.White);

                _game._spriteBatch.End();
            }

            public void Update(GameTime gameTime)
            {
                var newKeyboardState = Keyboard.GetState();
                var newMouseState = Mouse.GetState();

                if (
                    newMouseState.LeftButton == ButtonState.Released &&
                    _game._oldMouseState.LeftButton == ButtonState.Pressed &&
                    _game._oldMouseState.X == newMouseState.X &&
                    _game._oldMouseState.Y == newMouseState.Y)
                {

                    _game._state = new InitializeNewGameState(_game);
                }

                _game._oldMouseState = newMouseState;
                _game._oldKeyboardState = newKeyboardState;
            }
        }
    }
}
