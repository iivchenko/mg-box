namespace KenneyAsteroids.Engine
{
    public static class GameExtensions
    {
        public static void RunSafe(this Game game)
        {
            using (game) { game.Run(); }
        }
    }
}
