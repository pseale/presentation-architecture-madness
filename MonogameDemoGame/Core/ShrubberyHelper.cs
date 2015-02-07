using Microsoft.Xna.Framework;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core
{
    public static class ShrubberyHelper
    {
        public static Point Spawn(IBoundaryService boundaryService)
        {
            return boundaryService.CreatePointInBoundary();
        }
    }
}