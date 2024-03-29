﻿using System;
using System.Numerics;

using XVector = Microsoft.Xna.Framework.Vector2;
using XMatrix = Microsoft.Xna.Framework.Matrix;
using XRect = Microsoft.Xna.Framework.Rectangle;
using XColor = Microsoft.Xna.Framework.Color;
using XGameTime = Microsoft.Xna.Framework.GameTime;

namespace KenneyAsteroids.Engine.MonoGame
{
    public static class MonoGameExtensions
    {
        public static float ToDelta(this XGameTime time)
        {
            return (float)time.ElapsedGameTime.TotalSeconds;
        }

        public static XColor ToXna(this Color color)
        {
            return new XColor(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static XRect ToXna(this Rectangle rect)
        {
            return new XRect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static XRect? ToXna(this Rectangle? rect)
        {
            return rect.HasValue
                ? new XRect(rect.Value.X, rect.Value.Y, rect.Value.Width, rect.Value.Height)
                : new XRect?();
        }

        public static Color ToEngine(this XColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static XVector Transform(this XVector position, XMatrix matrix)
        {
            return new XVector((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41, (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
        }

        public static float ToRotation(this Vector2 direction)
        {
            return MathF.Atan2(direction.X, -direction.Y);
        }

        internal static XVector ToXnaVector(this Vector2 vector)
        {
            return new XVector(vector.X, vector.Y);
        }

        internal static Vector2 ToVector(this XVector vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
    }
}
