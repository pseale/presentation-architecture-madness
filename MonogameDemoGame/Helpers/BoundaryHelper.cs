using System;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Helpers
{
    public static class BoundaryHelper
    {
        public static bool OutOfBounds(float position, int gameBorder)
        {
            return Math.Abs(position) > gameBorder;
        }

        public static Point CreatePointInBoundary(IRandomNumberService random, int gameBorder)
        {
            return new Point(random.NextRandomNumberBetweenPositiveAndNegative(gameBorder), random.NextRandomNumberBetweenPositiveAndNegative(gameBorder));
        }
    }
}