namespace KenneyAsteroids.Engine.Eventing
{
    namespace Eventing
    {
        public interface IEventService
        {
            void Publish<TEvent>(TEvent @event)
                where TEvent : IEvent;
        }
    }
}
