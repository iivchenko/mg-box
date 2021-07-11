using BenchmarkDotNet.Attributes;
using KenneyAsteroids.Engine.Collisions;
using System;
using System.Linq;
using System.Numerics;

namespace KenneyAsteroids.Engine.Benchmark.Collisions
{
    public class CollisionSystemBenchmark
    {
        private readonly Random _random;
        private readonly CollisionSystem _system;

        private IBody[] _bodies_10;
        private IBody[] _bodies_100;
        private IBody[] _bodies_1000;
        private IBody[] _bodies_10000;

        public CollisionSystemBenchmark()
        {
            _random = new Random();
            _system = new CollisionSystem(Enumerable.Empty<IRule>());

            _bodies_10 =
                Enumerable
                    .Range(0, 9)
                    .Select(_ => CreateRandomBody())
                    .ToArray();

            _bodies_100 =
                Enumerable
                    .Range(0, 99)
                    .Select(_ => CreateRandomBody())
                    .ToArray();

            _bodies_1000 =
                Enumerable
                    .Range(0, 999)
                    .Select(_ => CreateRandomBody())
                    .ToArray();

            _bodies_10000 =
                Enumerable
                    .Range(0, 9999)
                    .Select(_ => CreateRandomBody())
                    .ToArray();
        }

        [Benchmark]
        public void ApplyCollisions_10bodies()
        {
            _system.ApplyCollisions(_bodies_10);
        }

        [Benchmark]
        public void ApplyCollisions_100bodies()
        {
            _system.ApplyCollisions(_bodies_100);
        }

        [Benchmark]
        public void ApplyCollisions_1000bodies()
        {
            _system.ApplyCollisions(_bodies_1000);
        }

        [Benchmark]
        public void ApplyCollisions_10000bodies()
        {
            _system.ApplyCollisions(_bodies_10000);
        }

        private IBody CreateRandomBody()
        {
            return new Body
            {
                Width = _random.Next(0, int.MaxValue),
                Height = _random.Next(0, int.MaxValue),
                Position = new Vector2((float)_random.NextDouble(), (float)_random.NextDouble()),
                Origin = Vector2.Zero
            };
        }

        private class Body : IBody
        {
            public Vector2 Position { get; set; }
            public Vector2 Origin { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
        }
    }
}
