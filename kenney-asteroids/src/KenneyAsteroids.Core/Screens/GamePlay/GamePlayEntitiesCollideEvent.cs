using KenneyAsteroids.Engine.Rules;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public sealed class GamePlayEntitiesCollideEvent<TBody1, TBody2> : IEvent
    {
        public GamePlayEntitiesCollideEvent(TBody1 body1, TBody2 body2)
        {
            Id = Guid.NewGuid();
            Body1 = body1;
            Body2 = body2;
        }

        public Guid Id { get; }
        public TBody1 Body1 { get; }
        public TBody2 Body2 { get; }
    }
}
