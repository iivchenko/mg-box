using System;

namespace TransformedCollision.Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new TransformedCollisionGame();
            
            game.Run();
        }
    }
}
