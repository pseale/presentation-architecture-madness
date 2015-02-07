using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Services
{
    public interface IBoundaryService
    {
        bool OutOfBounds(float position);
        Point CreatePointInBoundary();
    }
}