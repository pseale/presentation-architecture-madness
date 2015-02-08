using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain;

namespace MonogameDemoGame.Core
{
    public static class PlayerHelper
    {
        public static Player Spawn(Point midpoint)
        {
            return new Player(new Point(midpoint.X, midpoint.Y), new Vector2(0f, 1f));
        }
    }
}