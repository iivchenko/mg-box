namespace KenneyAsteroids.Engine.Eventing
{
    namespace Eventing
    {
        public interface IEventBus
        {
            void Register<TEvent>(params IEventHandler<TEvent>[] handlers)
                where TEvent : IEvent;
        }
    }
}
