using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameDemoGame.Helpers
{
    public static class DrawHelper
    {
        public static void DrawEntityWithRotation(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Vector2 direction, int playerSize, int halfPlayerSize)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, playerSize, playerSize),
                new Color(Color.White, 1f), MathHelper.ConvertToAngleInRadians(direction), new Vector2(halfPlayerSize, halfPlayerSize), 1.0f, SpriteEffects.None, 1);
        }

        public static void DrawEntity(SpriteBatch spriteBatch, Texture2D texture, Vector2 position)
        {
            spriteBatch.Draw(texture, position);
        }

        public static void EndFrame(SpriteBatch spriteBatch, Action drawAction)
        {
            spriteBatch.End();
            drawAction();
        }

        public static void InitializeFrame(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Point cameraPosition, int widthMidpoint, int heightMidpoint, Color backgroundColor)
        {
            graphicsDevice.Clear(backgroundColor);

            //http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
            var transform = Matrix.CreateTranslation(new Vector3(-cameraPosition.X, -cameraPosition.Y, 0))*
                            Matrix.CreateRotationZ(0)*
                            Matrix.CreateScale(new Vector3(1, 1, 1))*
                            Matrix.CreateTranslation(new Vector3(widthMidpoint, heightMidpoint, 0));
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
        }
    }
}