using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameDemoGame.Helpers
{
    public static class ContentHelper
    {
        public static Texture2D CreateSquareTexture(GraphicsDevice graphicsDevice, Color color, int size)
        {
            var texture = new Texture2D(graphicsDevice, size, size);

            texture.SetData(
                Enumerable.Range(0, size * size)
                    .Select(cell => color)
                    .ToArray());

            return texture;
        }

        public static Texture2D LoadTextureFromFile(ContentManager contentManager, string fileName)
        {
            return contentManager.Load<Texture2D>(fileName);
        }

        public static SpriteFont LoadFontByName(ContentManager contentManager, string fontName)
        {
            return contentManager.Load<SpriteFont>(fontName);
        }
    }
}