using System;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Helpers
{
    public static class MathHelper
    {
        public static Vector2 ShrinkVectorTo1Magnitude(Vector2 vector)
        {
            var magnitude = 1f / (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return  vector * magnitude;
        }

        public static float ConvertToAngleInRadians(Vector2 direction)
        {
            return (float)Math.Atan2(direction.Y, direction.X);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float Deg2Rad = ((float)(2 * Math.PI)/ 360f);
            float sin = (float)Math.Sin(degrees * Deg2Rad);
            float cos = (float)Math.Cos(degrees * Deg2Rad);

            float tx = v.X;
            float ty = v.Y;
            v.X = (cos * tx) - (sin * ty);
            v.Y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}