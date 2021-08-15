namespace KenneyAsteroids.Engine.Content
{
    public interface IContentProvider
    {
        TContent Load<TContent>(string path)
            where TContent : class;
    }
}
