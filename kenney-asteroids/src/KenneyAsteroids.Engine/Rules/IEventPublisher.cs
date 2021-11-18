namespace KenneyAsteroids.Engine.Rules
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}

