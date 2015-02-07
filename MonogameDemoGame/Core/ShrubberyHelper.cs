using System;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core
{
    public static class ShrubberyHelper
    {
        public static Point Spawn(IRandomNumberService randomNumberService, int gameBorder)
        {
            return BoundaryHelper.CreatePointInBoundary(randomNumberService, gameBorder);
        }
    }
}