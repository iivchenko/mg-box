namespace KenneyAsteroids.Engine.Entities
{
    public interface IEntity<TId> : IEntity
    {
        TId Id { get; }
    }

    public interface IEntity
    {
    }
}
