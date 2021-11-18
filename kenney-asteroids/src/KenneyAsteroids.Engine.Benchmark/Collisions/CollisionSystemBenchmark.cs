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

        public CollisionSystemBenchmark()
        {
            _random = new Random();
            _system = new CollisionSystem();

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
        }

        [Benchmark]
        public void ApplyCollisions_10bodies()
        {
            _system.EvaluateCollisions(_bodies_10);
        }

        [Benchmark]
        public void ApplyCollisions_100bodies()
        {
            _system.EvaluateCollisions(_bodies_100);
        }

        [Benchmark]
        public void ApplyCollisions_1000bodies()
        {
            _system.EvaluateCollisions(_bodies_1000);
        }

        private IBody CreateRandomBody()
        {
            var width = _random.Next(1, 100);
            var height = _random.Next(1, 100);
            var data = new Color[unchecked(width * height)];
            for(var i = 0; i < width * height; i++)
            {
                data[i] = new Color((byte)_random.Next(), (byte)_random.Next(), (byte)_random.Next(), (byte)_random.Next());
            }

            return new Body
            {
                Width = width,
                Height = height,
                Position = new Vector2((float)_random.NextDouble(), (float)_random.NextDouble()),
                Scale = new Vector2((float)_random.NextDouble(), (float)_random.NextDouble()),
                Rotation = (float)_random.NextDouble(),
                Origin = Vector2.Zero,
                Data = data
            };
        }

        private class Body : IBody
        {
            public Vector2 Position { get; set; }
            public Vector2 Origin { get; set; }
            public Vector2 Scale { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
            public float Rotation { get; set; }
            public Color[] Data { get; set; }
        }
    }
}
