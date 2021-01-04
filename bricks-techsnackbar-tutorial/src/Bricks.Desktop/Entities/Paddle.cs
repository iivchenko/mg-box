using Bricks.Desktop.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bricks.Desktop.Entities
{
    public sealed class Paddle : Entity
    {
        private readonly Texture2D _sprite;
        private readonly SpriteBatch _spriteBatch;
        private readonly Vector2 _origin;

        public Paddle(Vector2 position, SpriteBatch spriteBatch, Texture2D sprite, IWorld world)
            : base (world)
        {
            Position = position;

            _sprite = sprite;
            _spriteBatch = spriteBatch;

            _origin = Vector2.Zero;

            Width = _sprite.Width;
            Height = _sprite.Height;
        }

        public float Width { get; }

        public float Height { get; }

        public override void Update(GameTime gameTime)
        {
            Body = new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);

            // TODO: Implement controller for the paddle. Implement collision with border
            //Move(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(
               _sprite,
               Position,
               null,
               Color.White,
               0,
               _origin,
               1.0f,
               SpriteEffects.None,
               0);
        }

        public void MoveLeft()
        {
            Position = new Vector2(Position.X - 5, Position.Y);
        }

        public void MoveRight()
        {
            Position = new Vector2(Position.X + 5, Position.Y);
        }

        public void MoveTo(float x)
        {
            Position = new Vector2(x, Position.Y);
        }

        internal override void CollidedBy(Entity entity)
        {
        }
    }
}
