using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Services;

namespace MonogameDemoGame.Core.Domain.Spawning
{
    public static class LobGameStarter
    {
        public static InitialGameState CreateInitialGameState(Point midpoint, int gameBorder, int NumberOfShrubbery, int NumberOfEnemies)
        {
            var state = new InitialGameState();
            
            state.GameBorder = gameBorder;
            state.Camera = SpawnCamera();
            state.Player = SpawnPlayer(midpoint);
            state.Shrubbery = SpawnShrubbery(NumberOfShrubbery, gameBorder);
            state.Enemies = SpawnEnemies(NumberOfEnemies, gameBorder);
            
            return state;
        }

        private static CameraDto SpawnCamera()
        {
            return new CameraDto { Position = new Point() };
        }

        private static PlayerDto SpawnPlayer(Point position)
        {
            return new PlayerDto()
            {
                Position = new Point(position.X, position.Y), 
                FacingDirection = new Vector2(0f, 1f)
            };
        }

        private static ShrubberyDto[] SpawnShrubbery(int numberOfShrubbery, int gameBorder)
        {
            var list = new List<ShrubberyDto>();
            for (int i = 0; i < numberOfShrubbery; i++)
            {
                list.Add(new ShrubberyDto() { Position = CreatePointInBoundary(gameBorder)});
            }

            return list.ToArray();
        }

        private static EnemyDto[] SpawnEnemies(int numberOfEnemies, int gameBorder)
        {
            var list = new List<EnemyDto>();
            
            for (int i = 0; i < numberOfEnemies; i++)
            {
                list.Add(new EnemyDto()
                {
                    Position = CreatePointInBoundary(gameBorder).ToVector2(), 
                    FacingDirection = GenerateEnemyDirection()
                });
            }

            return list.ToArray();
        }

        private static Vector2 GenerateEnemyDirection()
        {
            int x = 0;
            int y = 0;
            if (RandomNumberService.GetRandomBool())
                x = RandomNumberService.GenerateRandomNegativeOrPositiveOne();
            else
                y = RandomNumberService.GenerateRandomNegativeOrPositiveOne();

            var direction = new Vector2(x, y);
            return direction;
        }

        private static Point CreatePointInBoundary(int gameBorder)
        {
            return new Point(
                RandomNumberService.NextRandomNumberBetweenPositiveAndNegative(gameBorder),
                RandomNumberService.NextRandomNumberBetweenPositiveAndNegative(gameBorder));
        }
    }
}