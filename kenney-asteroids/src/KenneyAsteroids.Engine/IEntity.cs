namespace KenneyAsteroids.Engine
{
    public interface IEntity<TId> : IEntity
    {
        TId Id { get; }
    }

    public interface IEntity
    {
    }
}
