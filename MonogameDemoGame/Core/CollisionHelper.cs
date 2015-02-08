using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core
{
    public static class CollisionHelper
    {
        public static CollisionSplash Spawn(Bullet bullet)
        {
            return new CollisionSplash(bullet.Position, new Vector2() - bullet.Direction);
        }
    }
}