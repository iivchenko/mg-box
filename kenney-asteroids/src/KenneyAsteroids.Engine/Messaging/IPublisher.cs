namespace KenneyAsteroids.Engine.Messaging
{
    public interface IPublisher
    {
        void Publish<TMessage>(TMessage message)
            where TMessage : IMessage;
    }
}

