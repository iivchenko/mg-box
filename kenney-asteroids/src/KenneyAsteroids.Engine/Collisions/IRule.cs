namespace KenneyAsteroids.Engine.Collisions
{
    public interface IRule
    {
        bool Match(IBody body1, IBody body2);

        void Action(IBody body1, IBody body2);
    }
}
