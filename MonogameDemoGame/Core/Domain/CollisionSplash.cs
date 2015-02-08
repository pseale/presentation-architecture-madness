using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain
{
    public class CollisionSplash
    {
        private readonly int _duration;

        public CollisionSplash(Vector2 position, Vector2 direction, int duration)
        {
            _duration = duration;
            Position = position;
            Direction = direction;
        }

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public int SplashCounter { get; private set; }

        public void Update()
        {
            SplashCounter++;
            Position = Position + Direction;
        }

        public bool ShouldBeDeleted()
        {
            return SplashCounter > _duration;
        }
    }
}