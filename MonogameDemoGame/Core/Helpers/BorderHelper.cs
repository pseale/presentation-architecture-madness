using Microsoft.Xna.Framework;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core.Helpers
{
    public static class BorderHelper
    {
        public static Point CreatePointInBoundary(int gameBorder)
        {
            return new Point(
                RandomNumberService.NextRandomNumberBetweenPositiveAndNegative(gameBorder),
                RandomNumberService.NextRandomNumberBetweenPositiveAndNegative(gameBorder));
        }
    }
}