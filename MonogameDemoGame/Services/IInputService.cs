using Microsoft.Xna.Framework;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Services
{
    public interface IInputService
    {
        InputStruct ProcessInput(Point playerPosition, Point cameraPosition);
        bool UserIsTryingToExit();
    }
}