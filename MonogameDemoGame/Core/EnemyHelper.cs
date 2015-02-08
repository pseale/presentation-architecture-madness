using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core
{
    public static class EnemyHelper
    {
        public static IEnumerable<Enemy> SpawnEnemies(IBoundaryService boundaryService, int randomSeedForEnemies, int numberOfEnemiesToSpawn)
        {
            var list = new List<Enemy>();
            var randomNumberService = new RandomNumberService(randomSeedForEnemies);
            foreach (var i in Enumerable.Range(1, numberOfEnemiesToSpawn))
            {
                list.Add(new Enemy(boundaryService.CreatePointInBoundary().ToVector2(),
                    GenerateEnemyDirection(randomNumberService)));
            }
            return list;
        }

        public static Vector2 GenerateEnemyDirection(IRandomNumberService randomNumberService)
        {
            int x = 0;
            int y = 0;
            if (randomNumberService.GetRandomBool())
                x = randomNumberService.GenerateRandomNegativeOrPositiveOne();
            else
                y = randomNumberService.GenerateRandomNegativeOrPositiveOne();

            var direction = new Vector2(x, y);
            return direction;
        }

        public static void DrawEnemies(IDrawService drawService, IEnumerable<Enemy> enemies, Texture2D enemyTexture, int playerSize, int halfPlayerSize)
        {
            foreach (var enemy in enemies)
            {
                drawService.DrawEntityWithRotation(enemyTexture, enemy.Position, enemy.Direction, playerSize, halfPlayerSize);
            }
        }
    }
}