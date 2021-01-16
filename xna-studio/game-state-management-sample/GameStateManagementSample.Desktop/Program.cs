using GameStateManagement;
using System;

namespace GameStateManagementSample.Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (GameStateManagementGame game = new GameStateManagementGame())
            {
                game.Run();
            }
        }
    }
}
