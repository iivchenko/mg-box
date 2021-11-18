namespace KenneyAsteroids.Engine.Rules
{
    public interface IRule<TEvent>
        where TEvent : IEvent
    {
        bool ExecuteCondition(TEvent @event);

        void ExecuteAction(TEvent @event);
    }
}
