using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

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
            if (Visible == true)
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

        public bool Move(Wall wall, Paddle paddle)
        {
            if (Visible == false)
            {
                return false;
            }
            X = X + XVelocity;
            Y = Y + YVelocity;

            //check for wall hits
            if (X < 1)
            {
                X = 1;
                XVelocity = XVelocity * -1;
                PlaySound(_wallBounceSfx);
            }
            if (X > ScreenWidth - Width + 5)
            {
                X = ScreenWidth - Width + 5;
                XVelocity = XVelocity * -1;
                PlaySound(_wallBounceSfx);
            }
            if (Y < 1)
            {
                Y = 1;
                YVelocity = YVelocity * -1;
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

            Rectangle paddleRect = new Rectangle((int)paddle.X, (int)paddle.Y, (int)paddle.Width, (int)paddle.Height);
            Rectangle ballRect = new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
            if (HitTest(paddleRect, ballRect))
            {
                PlaySound(_paddleBounceSfx);
                int offset = Convert.ToInt32((paddle.Width - (paddle.X + paddle.Width - X + Width / 2)));
                offset = offset / 5;
                if (offset < 0)
                {
                    offset = 0;
                }
                switch (offset)
                {
                    case 0:
                        XVelocity = -6;
                        break;
                    case 1:
                        XVelocity = -5;
                        break;
                    case 2:
                        XVelocity = -4;
                        break;
                    case 3:
                        XVelocity = -3;
                        break;
                    case 4:
                        XVelocity = -2;
                        break;
                    case 5:
                        XVelocity = -1;
                        break;
                    case 6:
                        XVelocity = 1;
                        break;
                    case 7:
                        XVelocity = 2;
                        break;
                    case 8:
                        XVelocity = 3;
                        break;
                    case 9:
                        XVelocity = 4;
                        break;
                    case 10:
                        XVelocity = 5;
                        break;
                    default:
                        XVelocity = 6;
                        break;
                }
                YVelocity = YVelocity * -1;
                Y = paddle.Y - Height + 1;
                return true;
            }
            bool hitBrick = false;
            for (int i = 0; i < 7; i++)
            {
                if (hitBrick == false)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Brick brick = wall.BrickWall[i, j];
                        if (brick.Visible)
                        {
                            Rectangle brickRect = new Rectangle((int)brick.Position.X, (int)brick.Position.Y, (int)brick.Width, (int)brick.Height);
                            if (HitTest(ballRect, brickRect))
                            {
                                PlaySound(_brickBounceSfx);
                                brick.Visible = false;
                                Score = Score + 7 - i;
                                YVelocity = YVelocity * -1;
                                bricksCleared++;
                                hitBrick = true;
                                break;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static void PlaySound(SoundEffect sound)
        {
            float volume = 1;
            float pitch = 0.0f;
            float pan = 0.0f;
            sound.Play(volume, pitch, pan);
        }

        public static bool HitTest(Rectangle r1, Rectangle r2)
        {
            if (Rectangle.Intersect(r1, r2) != Rectangle.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
