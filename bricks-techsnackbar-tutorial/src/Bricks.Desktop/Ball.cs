using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bricks.Desktop
{
    public sealed class Ball
    {
        private readonly Texture2D _sprite;
        private readonly SoundEffect _startSfx;
        private readonly SoundEffect _wallBounceSfx;
        private readonly SoundEffect _paddleBounceSfx;
        private readonly SoundEffect _brickBounceSfx;

        public float X { get; set; }
        public float Y { get; set; }
        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public float Rotation { get; set; }
        public bool UseRotation { get; set; }
        public float ScreenWidth { get; set; } //width of game screen
        public float ScreenHeight { get; set; } //height of game screen
        public bool Visible { get; set; }  //is ball visible on screen
        public int Score { get; set; }
        public int bricksCleared { get; set; } //number of bricks cleared this level

        private readonly SpriteBatch _spriteBatch;  //allows us to write on backbuffer when we need to draw self

        public Ball(
            float screenWidth, 
            float screenHeight, 
            SpriteBatch spriteBatch, 
            Texture2D sprite,
            SoundEffect startSfx,
            SoundEffect wallBounceSfx,
            SoundEffect paddleBounceSfx,
            SoundEffect brickBounceSfx)
        {
            X = 0;
            Y = 0;
            XVelocity = 0;
            YVelocity = 0;
            Rotation = 0;
            _sprite = sprite;
            _startSfx = startSfx;
            _wallBounceSfx = wallBounceSfx;
            _paddleBounceSfx = paddleBounceSfx;
            _brickBounceSfx = brickBounceSfx;

            Width = _sprite.Width;
            Height = _sprite.Height;

            _spriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight; 
            Visible = false;
            Score = 0;
            bricksCleared = 0;
            UseRotation = true;
        }

        public void Draw()
        {
            if (Visible)
            {
                if (UseRotation)
                {
                    Rotation += .1f;
                    if (Rotation > 3 * Math.PI)
                    {
                        Rotation = 0;
                    }
                }
                _spriteBatch.Draw(_sprite, new Vector2(X, Y), null, Color.White, Rotation, new Vector2(Width / 2, Height / 2), 1.0f, SpriteEffects.None, 0);
            }
        }

        public void Launch(float x, float y, float xVelocity, float yVelocity)
        {
            if (Visible)
            {
                return;
            }

            PlaySound(_startSfx);

            Visible = true;
            X = x;
            Y = y;
            XVelocity = xVelocity;
            YVelocity = yVelocity;
        }

        public bool Move(IList<Brick> wall, Paddle paddle)
        {
            if (!Visible)
            {
                return false;
            }

            X += XVelocity;
            Y += YVelocity;

            //check for wall hits
            if (X < 1)
            {
                X = 1;
                XVelocity *= -1;
                PlaySound(_wallBounceSfx);
            }
            if (X > ScreenWidth - Width + 5)
            {
                X = ScreenWidth - Width + 5;
                XVelocity *= -1;
                PlaySound(_wallBounceSfx);
            }
            if (Y < 1)
            {
                Y = 1;
                YVelocity *= -1;
                PlaySound(_wallBounceSfx);
            }
            if (Y > ScreenHeight)
            {
                Visible = false;
                Y = 0;
                PlaySound(_wallBounceSfx);
                return false;
            }

            //check for paddle hit
            //paddle is 70 pixels. we'll logically divide it into segments that will determine the angle of the bounce

            Rectangle ballRect = new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
            if (ballRect.Intersects(paddle.Body))
            {
                PlaySound(_paddleBounceSfx);
                int offset = Convert.ToInt32((paddle.Width - (paddle.Position.X + paddle.Width - X + Width / 2)));
                offset = offset / 5;
                if (offset < 0)
                {
                    offset = 0;
                }
                XVelocity = offset switch
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
                YVelocity *= -1;
                Y = paddle.Position.Y - Height + 1;
                return true;
            }

            var brick = wall.FirstOrDefault(x => x.Body.Intersects(ballRect));

            if (brick != null)
            {
                wall.Remove(brick);
                PlaySound(_brickBounceSfx);
                Score += brick.Score;
                YVelocity *= -1;
                bricksCleared++;
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
    }
}
