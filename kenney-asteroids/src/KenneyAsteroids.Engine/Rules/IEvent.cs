using System;

namespace KenneyAsteroids.Engine.Rules
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
