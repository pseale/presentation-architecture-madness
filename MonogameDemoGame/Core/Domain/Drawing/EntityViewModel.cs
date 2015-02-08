using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain.Drawing
{
    public class EntityViewModel
    {
        public EntityType Type { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set; }
        public bool HasRotation { get; set; }
    }
}