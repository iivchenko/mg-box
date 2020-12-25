using Microsoft.Xna.Framework;
using System;

namespace Bricks.Desktop.Engine.StateManagement
{
    public abstract class State<TContext> : IState<TContext>
    {
        protected State(TContext context)
        {
            Context = context;
        }

        public event EventHandler<IState<TContext>> StateUpdate;

        protected TContext Context { get; }

        public abstract void Draw(GameTime time);

        public abstract void Update(GameTime time);

        protected void OnStateUpdate(IState<TContext> state)
        {
            StateUpdate.Invoke(this, state);
        }
    }
}