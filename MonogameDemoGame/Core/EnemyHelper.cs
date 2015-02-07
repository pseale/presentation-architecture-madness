using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameDemoGame.Helpers;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core
{
    public static class EnemyHelper
    {
        public static IEnumerable<EnemyStruct> SpawnEnemies(int randomSeedForEnemies, int numberOfEnemiesToSpawn, int gameBorder, int ticksToWaitAtBeginning, int enemyHealth)
        {
            var list = new List<EnemyStruct>();
            var random = new Random(randomSeedForEnemies);  //I want the exact same seed, not sure why honestly.
            foreach (var i in Enumerable.Range(1, numberOfEnemiesToSpawn))
            {
                list.Add(new EnemyStruct()
                {
                    Position = BoundaryHelper.CreatePointInBoundary(random, gameBorder).ToVector2(),
                    Direction = GenerateEnemyDirection(random),
                    State = EnemyState.DoingNothing,
                    TicksUntilDone = ticksToWaitAtBeginning,
                    Health = enemyHealth
                });
            }
            return list;
        }

        public static Vector2 GenerateEnemyDirection(Random random)
        {
            int x = 0;
            int y = 0;
            if (RandomHelper.GetRandomBool(random))
                x = RandomHelper.GenerateRandomNegativeOrPositiveOne(random);
            else
                y = RandomHelper.GenerateRandomNegativeOrPositiveOne(random);

            var direction = new Vector2(x, y);
            return direction;
        }

        public static void DrawEnemies(SpriteBatch spriteBatch, IEnumerable<EnemyStruct> enemies, Texture2D enemyTexture, int playerSize, int halfPlayerSize)
        {
            foreach (var enemy in enemies)
            {
                DrawHelper.DrawEntityWithRotation(spriteBatch, enemyTexture, enemy.Position, enemy.Direction, playerSize, halfPlayerSize);
            }
        }

        public static bool HasNoHealth(EnemyStruct enemy)
        {
            return enemy.Health <= 0;
        }

        public static void HurtEnemy(EnemyStruct enemy)
        {
            enemy.Health--;
        }

        public static void Update(EnemyStruct enemy, int enemyTicksToDoNothing, int enemyTicksToTurn, int enemyTicksToMove)
        {
            enemy.TicksUntilDone--;
            if (enemy.State == EnemyState.DoingNothing)
            {
                //do nothing

                if (enemy.TicksUntilDone == 0)
                    ChangeStateToMoving(enemy, enemyTicksToMove);
            }
            else if (enemy.State == EnemyState.Moving)
            {
                MoveEnemy(enemy);

                if (enemy.TicksUntilDone == 0)
                    ChangeStateToTurning(enemy, enemyTicksToTurn);
            }
            else if (enemy.State == EnemyState.Turning)
            {
                TurnEnemy(enemy);

                if (enemy.TicksUntilDone == 0)
                    ChangeStateToDoingNothing(enemy, enemyTicksToDoNothing);
            }
        }

        public static void TurnEnemy(EnemyStruct enemy)
        {
            enemy.Direction = enemy.Direction.Rotate(1);
        }

        public static void MoveEnemy(EnemyStruct enemy)
        {
            enemy.Position = enemy.Position + enemy.Direction;
        }

        public static void ChangeStateToDoingNothing(EnemyStruct enemy, int enemyTicksToDoNothing)
        {
            ChangeEnemyState(enemy, EnemyState.DoingNothing, enemyTicksToDoNothing);
        }

        public static void ChangeStateToTurning(EnemyStruct enemy, int enemyTicksToTurn)
        {
            ChangeEnemyState(enemy, EnemyState.Turning, enemyTicksToTurn);
        }

        public static void ChangeStateToMoving(EnemyStruct enemy, int enemyTicksToMove)
        {
            ChangeEnemyState(enemy, EnemyState.Moving, enemyTicksToMove);
        }

        public static void ChangeEnemyState(EnemyStruct enemy, EnemyState newState, int ticksUntilDone)
        {
            enemy.State = newState;
            enemy.TicksUntilDone = ticksUntilDone;
        }

    }
}