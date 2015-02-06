using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Structs
{
    internal class EnemyStruct
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public int TicksUntilDone { get; set; }

        public EnemyState State { get; set; }
        public int Health { get; set; }
    }
}