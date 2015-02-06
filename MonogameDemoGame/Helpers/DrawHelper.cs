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
    }
}