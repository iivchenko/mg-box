namespace KenneyAsteroids.Engine.Collisions
{
    public sealed class Collision
    {
        public Collision(IBody body1, IBody body2)
        {
            Body1 = body1;
            Body2 = body2;
        }

        public IBody Body1 { get; private set; }
        public IBody Body2 { get; private set; }
    }
}
