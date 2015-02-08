using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain;

namespace MonogameDemoGame.Helpers
{
    public static class CameraHelper
    {
        public static Camera Spawn(Point point)
        {
            return new Camera(new Point(point.X, point.Y));
        }
    }
}