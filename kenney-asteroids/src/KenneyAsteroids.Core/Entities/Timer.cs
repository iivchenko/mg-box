using KenneyAsteroids.Engine;
using KenneyAsteroids.Engine.Entities;
using KenneyAsteroids.Engine.Rules;
using System;
using System.Collections.Generic;

namespace KenneyAsteroids.Core.Entities
{
    public sealed class Timer : IEntity<Guid>, IUpdatable
    {
        private readonly IEventPublisher _publisher;

        private double _remain;

        public Timer(
            TimeSpan timeout,
            string tag, 
            IEventPublisher publisher)
        {
            _publisher = publisher;
            _remain = timeout.TotalSeconds;

            Timeout = timeout;
            Tags = new[] { tag };

            Id = Guid.NewGuid();
        }
        public TimeSpan Timeout { get; }

        public Guid Id { get; }

        public IEnumerable<string> Tags { get; }

        void IUpdatable.Update(float time)
        {       
            _remain -= time;

            if (_remain <= 0.0f)
            {
                _remain = Timeout.TotalSeconds;
                _publisher.Publish(new OnTimerEvent(this));
            }
        }
    }

    public sealed class OnTimerEvent : IEvent
    {
        public OnTimerEvent(Timer timer)
        {
            Timer = timer;

            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public Timer Timer { get; }
    }
}
