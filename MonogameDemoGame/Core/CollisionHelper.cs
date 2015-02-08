using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core
{
    public static class CollisionHelper
    {
        public static CollisionSplashStruct Spawn(Bullet bullet)
        {
            return new CollisionSplashStruct()
            {
                Position = bullet.Position,
                Direction = new Vector2() - bullet.Direction,
                SplashCounter = 0
            };
        }
    }
}