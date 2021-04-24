using System;

namespace KenneyAsteroids.Engine.Messaging
{
    public interface IMessage
    {
        Guid Id { get; }
    }
}
