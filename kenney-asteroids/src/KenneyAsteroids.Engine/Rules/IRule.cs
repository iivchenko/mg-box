namespace KenneyAsteroids.Engine.Rules
{
    public interface IRule<TEvent>
        where TEvent : IEvent
    {
        void Execute(TEvent @event);
    }
}
