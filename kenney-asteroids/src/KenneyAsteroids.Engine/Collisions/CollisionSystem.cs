using System.Collections.Generic;
using System.Linq;

namespace KenneyAsteroids.Engine.Collisions
{
    public sealed class CollisionSystem : ICollisionSystem
    {
        private readonly IReadOnlyList<IRule> _rules;

        public CollisionSystem(IEnumerable<IRule> rules)
        {
            _rules = rules.ToList();
        }

        public void ApplyCollisions(IEnumerable<IBody> bodies)
        {
            var array = bodies.ToArray();
            var collisions = new List<(IBody, IBody)>();

            for (var i = 0; i < array.Length; i++)
                for (var j = i + 1; j < array.Length; j++)
                {
                    var body1 = array[i];
                    var body2 = array[j];

                    var left1 = body1.Position.X - body1.Origin.X;
                    var right1 = body1.Position.X - body1.Origin.X + body1.Width;
                    var top1 = body1.Position.Y - body1.Origin.Y;
                    var bottom1 = body1.Position.Y - body1.Origin.Y + body1.Height;

                    var left2 = body2.Position.X - body2.Origin.X;
                    var right2 = body2.Position.X - body2.Origin.X + body2.Width;
                    var top2 = body2.Position.Y - body2.Origin.Y;
                    var bottom2 = body2.Position.Y - body2.Origin.Y + body2.Height;

                    if (left1 < right2 && left2 < right1 && top1 < bottom2 && top2 < bottom1)
                    {
                        collisions.Add((body1, body2));
                    }
                }

            foreach (var (body1, body2) in collisions)
            {
                _rules
                    .Where(x => x.Match(body1, body2))
                    .Iter(x => x.Action(body1, body2));
            }
        }
    }
}
