using UserInterfaceSample;
using System;

namespace GameStateManagementSample.Desktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SampleGame())
            {
                game.Run();
            }
        }
    }
}
