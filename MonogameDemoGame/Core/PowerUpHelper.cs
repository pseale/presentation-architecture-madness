using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core
{
    public static class PowerUpHelper
    {
        public static Vector2 CalculateTextPosition(Point playerPosition)
        {
            return (playerPosition - new Point(50, -20)).ToVector2();
        }
    }
}