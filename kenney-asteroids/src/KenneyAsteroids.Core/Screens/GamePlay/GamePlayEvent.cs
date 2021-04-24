using KenneyAsteroids.Engine.Messaging;
using System;

namespace KenneyAsteroids.Core.Screens.GamePlay
{
    public abstract class GamePlayEvent : IMessage
    {
        protected GamePlayEvent()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}
