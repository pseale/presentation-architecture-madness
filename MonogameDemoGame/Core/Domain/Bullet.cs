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

        public bool ShouldBeDeleted(IBoundaryService boundaryService)
        {
            return boundaryService.OutOfBounds(Position.X)
                   || boundaryService.OutOfBounds(Position.Y);
        }

        public void Move()
        {
            Position = Position + Direction;
        }
    }
}