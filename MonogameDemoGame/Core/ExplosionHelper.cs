using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core
{
    public static class ExplosionHelper
    {
        public static Explosion Spawn(Enemy enemy, int fragmentsPerExplosion, int collisionFragmentMaxSpeed)
        {
            var fragments = new List<Vector2>();
        
            for (int i = 0; i < fragmentsPerExplosion; i++)
                fragments.Add(new Vector2(1, 0).Rotate(RandomNumberService.NextRandomNumber(360)) * RandomNumberService.NextRandomNumber(collisionFragmentMaxSpeed));

            return new Explosion(enemy.Position, fragments);
        }
    }
}