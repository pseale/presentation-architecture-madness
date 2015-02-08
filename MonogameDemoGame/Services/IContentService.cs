using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameDemoGame.Services
{
    public interface IContentService
    {
        Texture2D CreateSquareTexture(Color color, int size);
        Texture2D LoadTextureFromFile(string fileName);
        SpriteFont LoadFontByName(string fontName);
    }
}