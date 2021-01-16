using KenneyAsteroids.Engine;
using System;

namespace KenneyAsteroids.Desktop
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
