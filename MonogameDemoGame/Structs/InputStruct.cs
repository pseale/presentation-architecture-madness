using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Structs
{
    public class InputStruct
    {
        public Point MoveDirection { get; set; }
        public bool IsFiring { get; set; }
        public Vector2 PlayerFacingDirection { get; set; }
    }
}