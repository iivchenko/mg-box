using System;

namespace KenneyAsteroids.Engine.Collisions
{
    public sealed class LazyRule<TBody1, TBody2> : IRule
        where TBody1 : class, IBody
        where TBody2 : class, IBody
    {
        private readonly Func<TBody1, TBody2, bool> _match;
        private readonly Action<TBody1, TBody2> _action;

        public LazyRule(Func<TBody1, TBody2, bool> match, Action<TBody1, TBody2> action)
        {
            _match = match;
            _action = action;
        }

        public void Action(IBody body1, IBody body2)
        {
            if (body1 is TBody1 && body2 is TBody2)
                _action((TBody1)body1, (TBody2)body2);
            else if (body1 is TBody2 && body2 is TBody1)
                _action((TBody1)body2, (TBody2)body1);
            else
                throw new CollisionException($"Expected '{nameof(body1)}' '{nameof(body2)}' to be of types '{typeof(TBody1).FullName}', '{typeof(TBody2).FullName}' but was '{body1.GetType().FullName}', '{body2.GetType().FullName}'!");
        }

        public bool Match(IBody body1, IBody body2)
        {
            return
                body1 is TBody1 b11 && body2 is TBody2 b22 && _match(b11, b22) ||
                body1 is TBody2 b21 && body2 is TBody1 b12 && _match(b12, b21);
        }
    }
}
