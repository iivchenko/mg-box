using Bricks.Desktop.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bricks.Desktop.Entities
{
    public sealed class Ball : IEntity
    {
        private readonly Texture2D _sprite;
        private readonly SoundEffect _wallBounceSfx;
        private readonly SoundEffect _paddleBounceSfx;
        private readonly SoundEffect _brickBounceSfx;

        private readonly SpriteBatch _spriteBatch;

        private Vector2 _position;
        private Vector2 _velocity;

        private float _rotation;
        public float Height { get; set; }
        public float Width { get; set; }

        public float ScreenWidth { get; set; } //width of game screen
        public float ScreenHeight { get; set; } //height of game screen
        public int Score { get; set; }

        public Ball(
            Vector2 position,
            float screenWidth,
            float screenHeight,
            SpriteBatch spriteBatch,
            Texture2D sprite,
            SoundEffect wallBounceSfx,
            SoundEffect paddleBounceSfx,
            SoundEffect brickBounceSfx)
        {
            _position = position;
            _rotation = 0;
            _sprite = sprite;
            _wallBounceSfx = wallBounceSfx;
            _paddleBounceSfx = paddleBounceSfx;
            _brickBounceSfx = brickBounceSfx;

            Width = _sprite.Width;
            Height = _sprite.Height;

            _spriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Score = 0;

            _velocity = Vector2.Zero;
        }

        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        public bool Move(IList<Brick> wall, IList<IEntity> entities, Paddle paddle)
        {
            _position += Velocity;

            //check for wall hits
            if (_position.X < 1)
            {
                _position.X = 1;
                _velocity.X *= -1;
                PlaySound(_wallBounceSfx);
            }
            if (_position.X > ScreenWidth - Width + 5)
            {
                _position.X = ScreenWidth - Width + 5;
                _velocity.X *= -1;
                PlaySound(_wallBounceSfx);
            }
            if (_position.Y < 1)
            {
                _position.Y = 1;
                _velocity.Y *= -1;
                PlaySound(_wallBounceSfx);
            }
            if (_position.Y > ScreenHeight)
            {
                _position.Y = 0;
                PlaySound(_wallBounceSfx);
                return false;
            }

            //check for paddle hit
            //paddle is 70 pixels. we'll logically divide it into segments that will determine the angle of the bounce

            Rectangle ballRect = new Rectangle((int)_position.X, (int)_position.Y, (int)Width, (int)Height);
            if (ballRect.Intersects(paddle.Body))
            {
                PlaySound(_paddleBounceSfx);
                int offset = Convert.ToInt32((paddle.Width - (paddle.Position.X + paddle.Width - _position.X + Width / 2)));
                offset = offset / 5;
                if (offset < 0)
                {
                    offset = 0;
                }
                _velocity.X = offset switch
                {
                    0 => -6,
                    1 => -5,
                    2 => -4,
                    3 => -3,
                    4 => -2,
                    5 => -1,
                    6 => 1,
                    7 => 2,
                    8 => 3,
                    9 => 4,
                    10 => 5,
                    _ => 6,
                };
                _velocity.Y *= -1;
                _position.Y = paddle.Position.Y - Height + 1;
                return true;
            }

            var brick = wall.FirstOrDefault(x => x.Body.Intersects(ballRect));

            if (brick != null)
            {
                wall.Remove(brick);
                entities.Remove(brick);
                PlaySound(_brickBounceSfx);
                Score += brick.Score;
                _velocity.Y *= -1;
            }

            return true;
        }

        private static void PlaySound(SoundEffect sound)
        {
            const float volume = 1;
            const float pitch = 0.0f;
            const float pan = 0.0f;
            sound.Play(volume, pitch, pan);
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            _rotation += .1f;

            if (_rotation > 3 * Math.PI)
            {
                _rotation = 0;
            }

            _spriteBatch.Draw(_sprite, Position, null, Color.White, _rotation, new Vector2(Width / 2, Height / 2), 1.0f, SpriteEffects.None, 0);
        }
    }
}
