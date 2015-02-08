using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core
{
    public static class BulletHelper
    {
        public static IEnumerable<Bullet> Spawn(Player player, float bulletSpeed, int halfPlayerSize)
        {
            var list = new List<Bullet>();

            var xDelta = player.FacingDirection.X * bulletSpeed;
            var yDelta = player.FacingDirection.Y * bulletSpeed;
            foreach (var gunAngle in player.FiringAngles)
            {
                var angle = (int)RandomNumberService.GenerateRandomNumberClusteredTowardZero(gunAngle);
                if (RandomNumberService.GetRandomBool())
                    angle = -angle;

                var direction = new Vector2(xDelta, yDelta).Rotate(angle);

                var bullet = new Bullet(player.Position.ToVector2() + (halfPlayerSize*player.FacingDirection), direction);

                list.Add(bullet);
            }

            return list;
        }
    }
}