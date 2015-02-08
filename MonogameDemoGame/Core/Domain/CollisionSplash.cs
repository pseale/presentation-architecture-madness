using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain
{
    public class CollisionSplash
    {
        private const int CollisionSplashTicks = 10;

        public CollisionSplash(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public int SplashCounter { get; private set; }

        public void Update()
        {
            SplashCounter++;
        }

        public bool ShouldBeDeleted()
        {
            return SplashCounter > CollisionSplashTicks;
        }
    }
}