using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Core;
using MonogameDemoGame.Core.Domain;

namespace MonogameDemoGame.Structs
{
    public class PlayerStruct
    {
        public PlayerStruct()
        {
            GunAngles = new List<int>();
            Bullets = new List<Bullet>();
        }

        public Point Position { get; set; }
        public Vector2 FacingDirection { get; set; }
        public Point MoveDirection { get; set; }
        public List<int> GunAngles { get; set; }
        public bool IsFiring { get; set; }
        public List<Bullet> Bullets { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
    }
}