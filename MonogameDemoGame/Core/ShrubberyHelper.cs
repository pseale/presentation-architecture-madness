using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core
{
    public static class ShrubberyHelper
    {
        public static Shrubbery Spawn(Point point)
        {
            return new Shrubbery(point);
        }
    }
}