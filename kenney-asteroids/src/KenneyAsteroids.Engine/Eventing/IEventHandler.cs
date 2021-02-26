namespace KenneyAsteroids.Engine.Eventing
{
    namespace Eventing
    {
        public interface IEventHandler<TEvent>
            where TEvent : IEvent
        {
            void Handle(TEvent @event);
        }
    }
}
