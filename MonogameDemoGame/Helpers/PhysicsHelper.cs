using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Helpers
{
    public static class PhysicsHelper
    {
        public static bool Collides(Vector2 position1, int radius1, Vector2 position2, int radius2)
        {
            //collision horizontally
            if ((position1.X + radius1 > position2.X - radius2 && position1.X + radius1 < position2.X + radius2)
                || (position1.X - radius1 < position2.X + radius2 && position1.X - radius1 > position2.X - radius2))
            {
                //collision vertically
                if ((position1.Y + radius1 > position2.Y - radius2 && position1.Y + radius1 < position2.Y + radius2)
                    || (position1.Y - radius1 < position2.Y + radius2 && position1.Y - radius1 > position2.Y - radius2))
                {
                    return true;
                }
            }
            return false;
        }
    }
}