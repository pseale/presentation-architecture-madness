using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Structs
{
    public class PlayerStruct
    {
        public PlayerStruct()
        {
            GunAngles = new List<int>();
            Bullets = new List<BulletStruct>();
        }

        public Point Position { get; set; }
        public Vector2 FacingDirection { get; set; }
        public Point MoveDirection { get; set; }
        public List<int> GunAngles { get; set; }
        public bool IsFiring { get; set; }
        public List<BulletStruct> Bullets { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
    }
}