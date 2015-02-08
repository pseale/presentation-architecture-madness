using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain.Drawing
{
    public class TextViewModel
    {
        public bool ShouldShowText { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }
    }
}