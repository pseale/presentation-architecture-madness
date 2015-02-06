using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Structs
{
    public class ExplosionStruct
    {
        public Vector2 Position { get; set; }
        public int Ticks { get; set; }
        public List<Vector2> Fragments { get; set; }
    }
}