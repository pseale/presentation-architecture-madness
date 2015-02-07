using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core
{
    public static class BulletHelper
    {
        public static IEnumerable<BulletStruct> Spawn(Random random, List<int> gunAngles, Vector2 facingDirection, Point playerPosition, float bulletSpeed, int halfPlayerSize)
        {
            var list = new List<BulletStruct>();

            var xDelta = facingDirection.X * bulletSpeed;
            var yDelta = facingDirection.Y * bulletSpeed;
            foreach (var gunAngle in gunAngles)
            {
                var angle = (int)RandomHelper.GenerateRandomNumberClusteredTowardZero(random, gunAngle);
                if (RandomHelper.GetRandomBool(random))
                    angle = -angle;

                var direction = new Vector2(xDelta, yDelta).Rotate(angle);

                var bullet = new BulletStruct()
                {
                    Position = new Vector2(playerPosition.X + halfPlayerSize * facingDirection.X, playerPosition.Y + halfPlayerSize * facingDirection.Y),
                    Direction = direction
                };

                list.Add(bullet);
            }

            return list;
        }

        public static bool ShouldBeDeleted(BulletStruct bullet, int gameBorder)
        {
            return BoundaryHelper.OutOfBounds(bullet.Position.X, gameBorder) 
                   || BoundaryHelper.OutOfBounds(bullet.Position.Y, gameBorder);
        }

        public static void Move(BulletStruct bullet)
        {
            bullet.Position = new Vector2(bullet.Position.X + bullet.Direction.X, bullet.Position.Y + bullet.Direction.Y);
        }
    }
}