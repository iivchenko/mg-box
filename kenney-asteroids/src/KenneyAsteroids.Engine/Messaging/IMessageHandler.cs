namespace KenneyAsteroids.Engine.Messaging
{
    public interface IMessageHandler<TMessage>
        where TMessage : IMessage
    {
        void Execute(TMessage message);
    }
}
