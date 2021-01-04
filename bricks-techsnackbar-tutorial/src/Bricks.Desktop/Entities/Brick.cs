using Bricks.Desktop.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Bricks.Desktop.Entities
{
    public sealed class Brick : Entity
    {
        private readonly Vector2 _rotation = new Vector2(0, 0);
        private readonly Texture2D _sprite;
        private readonly Color _color;
        private readonly int _score;

        private readonly SpriteBatch _spriteBatch;

        public Brick(
            Vector2 position,
            Color color,
            SpriteBatch spriteBatch,
            Texture2D sprite,
            int score,
            IWorld world)
            : base (world)
        {
            Position = position;
            _sprite = sprite;
            _color = color;

            _spriteBatch = spriteBatch;

            Body = new Rectangle((int)Position.X, (int)Position.Y, _sprite.Width, _sprite.Height);

            _score = score;
        }

        public event EventHandler<int> Scored;

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(
               _sprite,
               Position,
               null,
               _color,
               0,
               _rotation,
               1.0f,
               SpriteEffects.None,
               0);
        }
        
        public override void Update(GameTime gameTime)
        {
        }

        internal override void CollidedBy(Entity entity)
        {
            if (entity is Ball)
            {
                Scored?.Invoke(this, _score);
            }
        }
    }
}
