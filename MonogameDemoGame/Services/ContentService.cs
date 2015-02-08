using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameDemoGame.Services
{
    public class ContentService : IContentService
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ContentManager _contentManager;

        public ContentService(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _contentManager = contentManager;
        }

        public Texture2D CreateSquareTexture(Color color, int size)
        {
            var texture = new Texture2D(_graphicsDevice, size, size);

            texture.SetData(
                Enumerable.Range(0, size * size)
                    .Select(cell => color)
                    .ToArray());

            return texture;
        }

        public Texture2D LoadTextureFromFile(string fileName)
        {
            return _contentManager.Load<Texture2D>(fileName);
        }

        public SpriteFont LoadFontByName(string fontName)
        {
            return _contentManager.Load<SpriteFont>(fontName);
        }
    }
}