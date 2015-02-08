using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Drawing
{
    public class EntityViewModel
    {
        public EntityType Type { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set; }
        public bool HasRotation { get; set; }
    }
}