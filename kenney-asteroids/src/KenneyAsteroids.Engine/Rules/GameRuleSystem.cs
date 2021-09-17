using System;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Engine.Rules
{
    public delegate IEnumerable<object> ServiceFactory(Type serviceType);

    public sealed class GameRuleSystem : IGameRuleSystem, IEventPublisher
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly IList<IEvent> _events;
        private readonly Type _handlerType;

        public GameRuleSystem(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;

            _handlerType = typeof(IRule<>);
            _events = new List<IEvent>();
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _events.Add(@event);
        }

        public void Update(float time)
        {
            var toProcess = _events.ToList();
            _events.Clear();

            toProcess
                .Select(x => new { MessageType = x.GetType(), Message = x })
                .Select(x => new { HandlerType = _handlerType.MakeGenericType(x.MessageType), Message = x.Message })
                .Select(x => new { Handers = (IEnumerable<dynamic>)_serviceFactory(x.HandlerType), Message = x.Message })
                .Iter(x => x.Handers.Where(handler => handler.ExecuteCondition((dynamic)x.Message)).Iter(handler => handler.ExecuteAction((dynamic)x.Message)));
        }
    }
}