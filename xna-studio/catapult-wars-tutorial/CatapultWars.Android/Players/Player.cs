using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatapultWars.Android.Players
{
    public class Player : DrawableGameComponent
    {
        // Constants used for calculating shot strength
        public const float MinShotStrength = 150;
        public const float MaxShotStrength = 400;

        protected CatapultGame curGame;
        protected SpriteBatch spriteBatch;

        public Player(Game game)
            : base(game)
        {
            curGame = (CatapultGame)game;
        }

        public Player(Game game, SpriteBatch screenSpriteBatch)
            : this(game)
        {
            spriteBatch = screenSpriteBatch;
        }

        public override void Initialize()
        {
            Score = 0;

            base.Initialize();
        }

        // Public variables used by Gameplay class
        public CatapultWars.Android.Catapult.Catapult Catapult { get; set; }

        public int Score { get; set; }

        public string Name { get; set; }

        public Player Enemy
        {
            set
            {
                Catapult.Enemy = value;
                Catapult.Self = this;
            }
        }

        public bool IsActive { get; set; }

        public override void Draw(GameTime gameTime)
        {
            // Draw related catapults
            Catapult.Draw(gameTime);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // Update catapult related to the player
            Catapult.Update(gameTime);
            base.Update(gameTime);
        }
    }
}