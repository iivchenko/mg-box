namespace KenneyAsteroids.Engine.Messaging
{
    public interface IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}
