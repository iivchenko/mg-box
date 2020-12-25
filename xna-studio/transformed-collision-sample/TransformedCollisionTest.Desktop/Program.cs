using System;

namespace TransformedCollisionTest.Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new TransformedCollisionTestGame();
            
            game.Run();
        }
    }
}
