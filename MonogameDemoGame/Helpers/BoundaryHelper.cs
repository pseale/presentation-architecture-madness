using System;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Helpers
{
    public static class BoundaryHelper
    {
        public static bool WithinBoundary(float position, int gameBorder)
        {
            return Math.Abs(position) > gameBorder;
        }

        public static Point CreatePointInBoundary(Random random, int gameBorder)
        {
            return new Point(RandomHelper.NextRandomNumberBetweenPositiveAndNegative(random, gameBorder), RandomHelper.NextRandomNumberBetweenPositiveAndNegative(random, gameBorder));
        }
    }
}