using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Services;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core
{
    public static class BulletSplashHelper
    {
        public static IEnumerable<Vector2> Spawn(IRandomNumberService randomNumberService, IEnumerable<CollisionSplashStruct> collisionSplashes,
            int NumberOfCollisionSplashParticlesToCreate, int MaximumSqrtOfAngleToThrowCollisionSplashParticleInDegrees)
        {
            var list = new List<Vector2>();

            foreach (var splash in collisionSplashes)
            {
                var directions = new List<int>();
                for (int i = 0; i < NumberOfCollisionSplashParticlesToCreate; i++)
                {
                    int randomNumber = randomNumberService.NextRandomNumberBetweenPositiveAndNegative(MaximumSqrtOfAngleToThrowCollisionSplashParticleInDegrees);

                    //like squaring, but keeping the negative-ness of the original number
                    directions.Add(randomNumber * Math.Abs(randomNumber));
                }

                foreach (var direction in directions)
                {
                    var particlePosition = splash.Position + (splash.Direction * splash.SplashCounter).Rotate(direction);
                    list.Add(particlePosition);
                }
            }

            return list;
        }
    }
}