using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Services;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core
{
    public static class BulletHelper
    {
        public static IEnumerable<Bullet> Spawn(IRandomNumberService randomNumberService, PlayerStruct player, float bulletSpeed, int halfPlayerSize)
        {
            var list = new List<Bullet>();

            var xDelta = player.FacingDirection.X * bulletSpeed;
            var yDelta = player.FacingDirection.Y * bulletSpeed;
            foreach (var gunAngle in player.GunAngles)
            {
                var angle = (int)randomNumberService.GenerateRandomNumberClusteredTowardZero(gunAngle);
                if (randomNumberService.GetRandomBool())
                    angle = -angle;

                var direction = new Vector2(xDelta, yDelta).Rotate(angle);

                var bullet = new Bullet(player.Position.ToVector2() + (halfPlayerSize*player.FacingDirection), direction);

                list.Add(bullet);
            }

            return list;
        }
    }
}