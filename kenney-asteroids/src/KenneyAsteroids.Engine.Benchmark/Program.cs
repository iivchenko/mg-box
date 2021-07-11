using BenchmarkDotNet.Running;

namespace KenneyAsteroids.Engine.Benchmark
{
    class Program
    {
        static void Main(string[] args) => 
            BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args);
    }
}
