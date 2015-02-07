using System;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Helpers;

namespace MonogameDemoGame.Core
{
    public static class ShrubberyHelper
    {
        public static Point Spawn(Random random, int gameBorder)
        {
            return BoundaryHelper.CreatePointInBoundary(random, gameBorder);
        }
    }
}