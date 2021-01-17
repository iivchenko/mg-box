using KenneyAsteroids.Core.Entities;
using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Screens;
using KenneyAsteroids.Engine.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Core.Screens
{
    public sealed class GamePlayScreen : GameScreen
    {
        private Viewport _viewport;
        private Texture2D _spriteSheet;
        private EntityFactory _factory;

        private IList<Entity> _entities;

        public override void LoadContent()
        {
            base.LoadContent();

            _viewport = ScreenManager.Game.GraphicsDevice.Viewport;
            _spriteSheet = ScreenManager.Game.Content.Load<Texture2D>("Sprites/AsteroidsSpriteSheet");
            _factory = new EntityFactory(_spriteSheet, ScreenManager.SpriteBatch);

            _entities = new List<Entity>
            {
                _factory.CreateShip(new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f))
            };
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            _entities.Update(gameTime);
            _entities.Where(IsOutOfScreen).Iter(Teleport);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.SpriteBatch.Begin();

            _entities.Draw(gameTime);

            ScreenManager.SpriteBatch.End();
        }

        private bool IsOutOfScreen(Entity entity)
        {
            return
                entity.Position.X + entity.Width / 2.0 < 0 || 
                entity.Position.X - entity.Width / 2.0 > _viewport.Width ||
                entity.Position.Y + entity.Height / 2.0 < 0 ||
                entity.Position.Y - entity.Height / 2.0 > _viewport.Height;
        }

        private void Teleport(Entity entity)
        {
            var x = entity.Position.X;
            var y = entity.Position.Y;

            if (entity.Position.X + entity.Width / 2.0f < 0)
            {
                x = _viewport.Width + entity.Width / 2.0f;
            }
            else if (entity.Position.X - entity.Width / 2.0f > _viewport.Width)
            {
                x = 0 - entity.Width / 2.0f;
            }

            if (entity.Position.Y + entity.Height / 2.0f < 0)
            {
                y = _viewport.Height + entity.Height / 2.0f;
            }
            else if (entity.Position.Y - entity.Height / 2.0f > _viewport.Height)
            {
                y = 0 - entity.Height / 2.0f;
            }

            entity.Position = new Vector2(x, y);
        }
    }
}
