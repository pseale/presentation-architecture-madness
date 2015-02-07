using System;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Helpers
{
    public static class CameraHelper
    {
        public static bool IsOutsideOfFlexZone(int distanceFromCamera, int noFlexZone)
        {
            return distanceFromCamera > noFlexZone || distanceFromCamera < -noFlexZone;
        }

        public static int FitToScreen(int cursorPosition, int boundary)
        {
            return Math.Max(Math.Min(cursorPosition, boundary), -boundary);
        }

        public static Point Spawn(Point midpoint)
        {
            return new Point(midpoint.X, midpoint.Y);
        }

        public static Point CalculateNewPosition(Point cameraPosition, Point playerPosition, Point moveDirection, int noFlexZone)
        {
            int x2 = cameraPosition.X;
            int y2 = cameraPosition.Y;

            if (CameraHelper.IsOutsideOfFlexZone(cameraPosition.X - playerPosition.X, noFlexZone))
                x2 += moveDirection.X;

            if (CameraHelper.IsOutsideOfFlexZone(cameraPosition.Y - playerPosition.Y, noFlexZone))
                y2 += moveDirection.Y;

            return new Point(x2, y2);
        }
    }
}