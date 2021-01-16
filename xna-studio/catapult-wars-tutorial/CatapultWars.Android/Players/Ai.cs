using CatapultWars.Android.Catapult;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CatapultWars.Android.Players
{
    public sealed class Ai : Player
    {
        private Random random;

        public Ai(Game game)
            : base(game)
        {
        }

        public Ai(Game game, SpriteBatch screenSpriteBatch)
            : base(game, screenSpriteBatch)
        {
            Catapult = new Catapult.Catapult(game, screenSpriteBatch,
                                "Textures/Catapults/Red/redIdle/redIdle",
                                new Vector2(600, 332), SpriteEffects.FlipHorizontally,
                                true);
        }

        public override void Initialize()
        {
            //Initialize randomizer
            random = new Random();

            Catapult.Initialize();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Check if it is time to take a shot
            if (Catapult.CurrentState == CatapultState.Aiming &&
                !Catapult.AnimationRunning)
            {
                // Fire at a random strength
                float shotVelocity =
                    random.Next((int)MinShotStrength, (int)MaxShotStrength);

                Catapult.ShotStrength = (shotVelocity / MaxShotStrength);
                Catapult.ShotVelocity = shotVelocity;
            }
            base.Update(gameTime);
        }
    }
}