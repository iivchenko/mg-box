using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KenneyAsteroids.Engine.Eventing
{
    namespace Eventing
    {
        public sealed class EventSystem : IEventBus, IEventService, IUpdatable
        {
            private readonly IDictionary<Type, IEnumerable<dynamic>> _eventHandlers;
            private readonly IList<IEvent> _events;

            public EventSystem()
            {
                _eventHandlers = new Dictionary<Type, IEnumerable<dynamic>>();
                _events = new List<IEvent>();
            }

            public void Register<TEvent>(params IEventHandler<TEvent>[] handlers) where TEvent : IEvent
            {
                _eventHandlers.Add(typeof(TEvent), handlers);
            }

            public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
            {
                _events.Add(@event);
            }

            public void Update(GameTime time)
            {
                _events.Iter(@event => _eventHandlers[@event.GetType()].Iter(handler => handler.Handle((dynamic)@event)));
                _events.Clear();
            }
        }
    }
}
