using System;

namespace KenneyAsteroids.Engine.Eventing
{
    namespace Eventing
    {
        public interface IEvent
        {
            Guid Id { get; }
        }
    }
}
