namespace KenneyAsteroids.Engine
{
    public static class GameExtensions
    {
        public static void RunSafe(this MonoGameGame game)
        {
            using (game) { game.Run(); }
        }
    }
}
