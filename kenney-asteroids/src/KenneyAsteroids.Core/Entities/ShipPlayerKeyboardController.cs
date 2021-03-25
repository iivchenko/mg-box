using KenneyAsteroids.Engine;
using Microsoft.Xna.Framework.Input;

namespace KenneyAsteroids.Core.Entities
{
    public class ShipPlayerKeyboardController : IEntity, IUpdatable
    {
        private readonly Ship _ship;

        public ShipPlayerKeyboardController(Ship ship)
        {
            _ship = ship;
        }

        public void Update(float time)
        {
            var action = ShipAction.None;
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.W))
            {
                action |= ShipAction.Accelerate;
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                action |= ShipAction.Left;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                action |= ShipAction.Right;
            }

            if (keyboard.IsKeyDown(Keys.Space))
            {
                action |= ShipAction.Fire;
            }

            _ship.Apply(action);
        }
    }
}
