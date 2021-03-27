using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Xna.Framework.Input;

namespace KenneyAsteroids.Core.Entities
{
    public class ShipPlayerKeyboardController : IEntity
    {
        private readonly Ship _ship;

        public ShipPlayerKeyboardController(Ship ship)
        {
            _ship = ship;
        }

        public void Handle(InputState input)
        {
            var action = ShipAction.None;
            var keyboard = input.CurrentKeyboardStates[0];

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
