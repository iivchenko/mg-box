using Bricks.Desktop.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop.Entities
{
    public sealed class Hud : IEntity
    {
        private readonly SpriteFont _font;
        private readonly Ball _ball;
        private readonly Texture2D _ballSprite;
        private readonly SpriteBatch _spriteBatch;

        private int _screenHeight;
        private int _screenWidth;

        private int _score;

        public Hud(
            SpriteFont font,
            int screenHeight,
            int screenWidth,
            Ball ball, 
            Texture2D ballSprite,
            SpriteBatch spriteBatch)
        {
            _font = font;
            _screenHeight = screenHeight;
            _screenWidth = screenWidth;
            _ball = ball;
            _ballSprite = ballSprite;
            _spriteBatch = spriteBatch;

            _score = 0;
        }

        public int BallsRemaining { get; set; }

        public Vector2 Position => Vector2.Zero;

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_ballSprite, new Vector2(15, 15), Color.White);

            string scoreMsg = "Score: " + _score.ToString("00000");

            var space = _font.MeasureString(scoreMsg);
            _spriteBatch.DrawString(_font, scoreMsg, new Vector2((_screenWidth - space.X) / 2, _screenHeight - 40), Color.White);
            _spriteBatch.DrawString(_font, BallsRemaining.ToString(), new Vector2(40, 10), Color.White);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void UpdateScore(int score)
        {
            _score += score;
        }
    }
}
