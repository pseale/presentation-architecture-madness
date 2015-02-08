using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameDemoGame.Services
{
    public interface IDrawService
    {
        void DrawEntityWithRotation(Texture2D texture, Vector2 position, Vector2 direction, int playerSize, int halfPlayerSize);
        void DrawEntity(Texture2D texture, Vector2 position);
        void InitializeFrame(Point cameraPosition, int widthMidpoint, int heightMidpoint, Color backgroundColor);
        void EndFrame(Action drawAction);
    }
}