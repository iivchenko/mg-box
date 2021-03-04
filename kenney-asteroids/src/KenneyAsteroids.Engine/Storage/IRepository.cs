namespace KenneyAsteroids.Engine.Storage
{
    public interface IRepository<TItem>
        where TItem : class, new()
    {
        TItem Read();

        void Update(TItem item);
    }
}
