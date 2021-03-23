namespace KenneyAsteroids.Engine.Eventing
{
    namespace Eventing
    {
        public interface IPublisher
        {
            void Publish<TEvent>(TEvent @event)
                where TEvent : IEvent;
        }
    }
}
