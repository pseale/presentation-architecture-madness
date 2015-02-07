using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core
{
    public static class ExplosionHelper
    {
        public static ExplosionUpdateResult Update(ExplosionStruct explosion, int explosionTicks)
        {
            explosion.Ticks++;
            
            if (explosion.Ticks > explosionTicks)
                return ExplosionUpdateResult.Remove;

            return ExplosionUpdateResult.DoNothing;
        }

        public static ExplosionStruct Spawn(Random random, EnemyStruct enemy, int fragmentsPerExplosion, int collisionFragmentMaxSpeed)
        {
            var explosionStruct = new ExplosionStruct() { Position = enemy.Position, Ticks = 0 };
            explosionStruct.Fragments = new List<Vector2>();
        
            for (int i = 0; i < fragmentsPerExplosion; i++)
                explosionStruct.Fragments.Add(new Vector2(1, 0).Rotate(RandomHelper.NextRandomNumber(random, 360)) * RandomHelper.NextRandomNumber(random, collisionFragmentMaxSpeed));

            return explosionStruct;
        }
    }
}