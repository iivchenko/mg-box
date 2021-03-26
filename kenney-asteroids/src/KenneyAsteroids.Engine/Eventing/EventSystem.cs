using System;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Engine.Eventing
{
    namespace Eventing
    {
        public delegate IEnumerable<object> ServiceFactory(Type serviceType);

        public sealed class EventSystem : IEventSystem, IPublisher
        {
            private readonly ServiceFactory _serviceFactory;
            private readonly IList<IEvent> _events;
            private readonly Type _handlerType;

            public EventSystem(ServiceFactory serviceFactory)
            {
                _serviceFactory = serviceFactory;

                _handlerType = typeof(IEventHandler<>);
                _events = new List<IEvent>();
            }

            public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
            {
                _events.Add(@event);
            }

            public void Update(float time)
            {
                _events
                    .Select(x => new { EventType = x.GetType(), Event = x })
                    .Select(x => new { HandlerType = _handlerType.MakeGenericType(x.EventType), Event = x.Event })
                    .Select(x => new { Handers = (IEnumerable<dynamic>)_serviceFactory(x.HandlerType), Event = x.Event })
                    .Iter(x => x.Handers.Iter(handler => handler.Handle((dynamic)x.Event)));

                _events.Clear();
            }
        }
    }
}
