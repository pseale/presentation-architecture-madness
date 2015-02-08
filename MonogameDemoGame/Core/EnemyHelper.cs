using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonogameDemoGame.Core.Domain;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core
{
    public static class EnemyHelper
    {
        public static void DrawEnemies(IDrawService drawService, IEnumerable<Enemy> enemies, Texture2D enemyTexture, int playerSize, int halfPlayerSize)
        {
            foreach (var enemy in enemies)
            {
                drawService.DrawEntityWithRotation(enemyTexture, enemy.Position, enemy.Direction, playerSize, halfPlayerSize);
            }
        }
    }
}