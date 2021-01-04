using Bricks.Desktop.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Bricks.Desktop.Entities
{
    public sealed class Ball : Entity
    {
        private readonly Texture2D _sprite;
        private readonly SoundEffect _paddleBounceSfx;
        private readonly SoundEffect _brickBounceSfx;

        private readonly SpriteBatch _spriteBatch;

        private float _rotation;
        public float Height { get; set; }
        public float Width { get; set; }

        public Ball(
            Vector2 position,
            SpriteBatch spriteBatch,
            Texture2D sprite,
            SoundEffect paddleBounceSfx,
            SoundEffect brickBounceSfx,
            IWorld world)
            : base (world)
        {
            Position = position;

            _rotation = 0;
            _sprite = sprite;
            _paddleBounceSfx = paddleBounceSfx;
            _brickBounceSfx = brickBounceSfx;

            Width = _sprite.Width;
            Height = _sprite.Height;

            _spriteBatch = spriteBatch;

            Velocity = Vector2.Zero;
        }

        private static void PlaySound(SoundEffect sound)
        {
            const float volume = 1;
            const float pitch = 0.0f;
            const float pan = 0.0f;
            sound.Play(volume, pitch, pan);
        }

        public override void Update(GameTime gameTime)
        {
            Body = new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);

            var collisions = Move(gameTime);
            var position = Position;
            var velocity = Velocity;
            Body = new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height);

            if (collisions.Any())
            {
                var paddle = collisions.Where(x => x is Paddle).FirstOrDefault();

                if (paddle != null)
                {
                    PlaySound(_paddleBounceSfx);
                    int offset = Convert.ToInt32((paddle.Body.Width - (paddle.Position.X + paddle.Body.Width - Position.X + Width / 2)));
                    offset /= 5;
                    if (offset < 0)
                    {
                        offset = 0;
                    }
                    velocity.X = offset switch
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
                    velocity.Y *= -1;
                    position.Y = paddle.Position.Y - Height + 1;
                }
                else
                {
                    PlaySound(_brickBounceSfx);
                    velocity.Y *= -1;
                }
            }

            Position = position;
            Velocity = velocity;
        }

        public override void Draw(GameTime gameTime)
        {
            _rotation += .1f;

            if (_rotation > 3 * Math.PI)
            {
                _rotation = 0;
            }

            _spriteBatch.Draw(_sprite, Position, null, Color.White, _rotation, new Vector2(Width / 2, Height / 2), 1.0f, SpriteEffects.None, 0);
        }

        internal override void CollidedBy(Entity entity)
        {
        }
    }
}
