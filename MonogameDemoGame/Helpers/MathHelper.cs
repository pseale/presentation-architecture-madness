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
    }
}