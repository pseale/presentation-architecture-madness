using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core
{
    public static class ShrubberyHelper
    {
        public static Shrubbery Spawn(IBoundaryService boundaryService)
        {
            return new Shrubbery(boundaryService.CreatePointInBoundary());
        }
    }
}