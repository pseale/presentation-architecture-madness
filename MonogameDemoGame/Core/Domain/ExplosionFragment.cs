using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain
{
    public class ExplosionFragment
    {
        public ExplosionFragment(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }

        public Vector2 Direction { get; private set; }
        public Vector2 Position { get; private set; }

        public void Update()
        {
            Position = Position + Direction;
        }
    }
}