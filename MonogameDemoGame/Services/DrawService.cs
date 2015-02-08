using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameDemoGame.Helpers;

namespace MonogameDemoGame.Services
{
    public class DrawService : IDrawService
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly GraphicsDevice _graphicsDevice;

        public DrawService(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;
        }

        public void DrawEntityWithRotation(Texture2D texture, Vector2 position, Vector2 direction, int playerSize, int halfPlayerSize)
        {
            _spriteBatch.Draw(texture, position, new Rectangle(0, 0, playerSize, playerSize),
                new Color(Color.White, 1f), MathHelper2.ConvertToAngleInRadians(direction), new Vector2(halfPlayerSize, halfPlayerSize), 1.0f, SpriteEffects.None, 1);
        }

        public void DrawEntity(Texture2D texture, Vector2 position)
        {
            _spriteBatch.Draw(texture, position);
        }

        public void InitializeFrame(Point cameraPosition, int widthMidpoint, int heightMidpoint, Color backgroundColor)
        {
            _graphicsDevice.Clear(backgroundColor);

            //http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
            var transform = Matrix.CreateTranslation(new Vector3(-cameraPosition.X, -cameraPosition.Y, 0))*
                            Matrix.CreateRotationZ(0)*
                            Matrix.CreateScale(new Vector3(1, 1, 1))*
                            Matrix.CreateTranslation(new Vector3(0, 0, 0));
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);
        }

        public void EndFrame(Action drawAction)
        {
            _spriteBatch.End();
            drawAction();
        }
    }
}