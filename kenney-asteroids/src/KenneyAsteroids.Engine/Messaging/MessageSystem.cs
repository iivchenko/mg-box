using System;
using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Engine.Messaging
{
    public delegate IEnumerable<object> ServiceFactory(Type serviceType);

    public sealed class MessageSystem : IMessageSystem, IPublisher
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly IList<IMessage> _messages;
        private readonly Type _handlerType;

        public MessageSystem(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;

            _handlerType = typeof(IMessageHandler<>);
            _messages = new List<IMessage>();
        }

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            _messages.Add(message);
        }

        public void Update(float time)
        {
            var toProcess = _messages.ToList();
            _messages.Clear();

            toProcess
                .Select(x => new { MessageType = x.GetType(), Message = x })
                .Select(x => new { HandlerType = _handlerType.MakeGenericType(x.MessageType), Message = x.Message })
                .Select(x => new { Handers = (IEnumerable<dynamic>)_serviceFactory(x.HandlerType), Message = x.Message })
                .Iter(x => x.Handers.Iter(handler => handler.Handle((dynamic)x.Message)));
        }
    }
}