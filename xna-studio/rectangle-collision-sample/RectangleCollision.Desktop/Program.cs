using System;

namespace RectangleCollision.Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new RectangleCollisionGame();
            
            game.Run();
        }
    }
}
