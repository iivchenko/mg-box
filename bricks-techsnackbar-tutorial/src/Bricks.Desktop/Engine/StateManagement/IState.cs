using Microsoft.Xna.Framework;
using System;

namespace Bricks.Desktop.Engine.StateManagement
{
    public interface IState<TContext>
    {

        event EventHandler<IState<TContext>> StateUpdate;

        void Update(GameTime time);

        void Draw(GameTime time);
    }
}