using System;
using System.Collections.Generic;
using System.Linq;

using Matrix = Microsoft.Xna.Framework.Matrix;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Color = Microsoft.Xna.Framework.Color;

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

                    if (
                        left1 < right2 && left2 < right1 && 
                        top1 < bottom2 && top2 < bottom1 &&
                        IntersectPixels(body1, body2))
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

        private static bool IntersectPixels(IBody body1, IBody body2)
        {
            var body1Matrix =
                Matrix.CreateTranslation(new Vector3(-body1.Origin.ToXnaVector(), 0.0f)) *
                Matrix.CreateScale(new Vector3(body1.Scale.ToXnaVector(), 1.0f)) *
                Matrix.CreateRotationZ(body1.Rotation) *
                Matrix.CreateTranslation(new Vector3(body1.Position.ToXnaVector(), 0.0f));

            var body2Matrix =
                Matrix.CreateTranslation(new Vector3(-body2.Origin.ToXnaVector(), 0.0f)) *
                Matrix.CreateScale(new Vector3(body2.Scale.ToXnaVector(), 1.0f)) *
                Matrix.CreateRotationZ(body2.Rotation) *
                Matrix.CreateTranslation(new Vector3(body2.Position.ToXnaVector(), 0.0f));

            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = body1Matrix * Matrix.Invert(body2Matrix);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < body1.Height; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < body1.Width; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < body2.Width &&
                        0 <= yB && yB < body2.Height)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = body1.Data[xA + yA * (int)body1.Width];
                        Color colorB = body2.Data[xB + yB * (int)body2.Width];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }
    }
}
