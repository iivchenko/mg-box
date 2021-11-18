using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Screens;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Core.Entities
{
    // TODO: Think on how to split keyboard and game pad correclty and by SOLID
    public class ShipPlayerController : IEntity
    {
        private readonly Ship _ship;

        public ShipPlayerController(Ship ship)
        {
            _ship = ship;
        }

        public IEnumerable<string> Tags => Enumerable.Empty<string>();

        public void Handle(InputState input)
        {
            var action = ShipAction.None;
            var keyboard = input.CurrentKeyboardStates[0];
            var gampad = input.CurrentGamePadStates[0];

            if (keyboard.IsKeyDown(Keys.W) ||
                gampad.IsButtonDown(Buttons.RightTrigger))
            {
                action |= ShipAction.Accelerate;
            }

            if (keyboard.IsKeyDown(Keys.A) ||
                gampad.IsButtonDown(Buttons.LeftThumbstickLeft) ||
                gampad.IsButtonDown(Buttons.DPadLeft))
            {
                action |= ShipAction.Left;
            }

            if (keyboard.IsKeyDown(Keys.D) ||
                gampad.IsButtonDown(Buttons.LeftThumbstickRight) ||
                gampad.IsButtonDown(Buttons.DPadRight))
            {
                action |= ShipAction.Right;
            }

            if (keyboard.IsKeyDown(Keys.Space) ||
                gampad.IsButtonDown(Buttons.A))
            {
                action |= ShipAction.Fire;
            }

            _ship.Apply(action);
        }
    }
}
