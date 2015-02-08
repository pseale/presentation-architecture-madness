using Microsoft.Xna.Framework;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core.Domain
{
    public class Bullet
    {
        public Bullet(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }

        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }

        public bool ShouldBeDeleted(LineOfBusinessApplication lob)
        {
            return lob.OutOfBounds(Position.X)
                   || lob.OutOfBounds(Position.Y);
        }

        public void Move()
        {
            Position = Position + Direction;
        }
    }
}