using KenneyAsteroids.Engine.Eventing.Eventing;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public abstract class GamePlayEvent : IEvent
    {
        protected GamePlayEvent()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}
