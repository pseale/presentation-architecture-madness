using System;

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
    }
}