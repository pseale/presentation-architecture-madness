using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain
{
    public class Camera
    {
        private const int NoFlexZone = 100;

        public Camera(Point position)
        {
            Position = position;
        }

        public Point Position { get; private set; }

        public void Move(Player player)
        {
            int x2 = Position.X;
            int y2 = Position.Y;

            if (IsOutsideOfFlexZone(Position.X - player.Position.X))
                x2 += player.MoveDirection.X;

            if (IsOutsideOfFlexZone(Position.Y - player.Position.Y))
                y2 += player.MoveDirection.Y;

            Position = new Point(x2, y2);
        }

        private bool IsOutsideOfFlexZone(int distanceFromCamera)
        {
            return distanceFromCamera > NoFlexZone || distanceFromCamera < -NoFlexZone;
        }
    }
}