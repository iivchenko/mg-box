using System;

namespace Bricks.Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new BricksGame())
                game.Run();
        }
    }
}
