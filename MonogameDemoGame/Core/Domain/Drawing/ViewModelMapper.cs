using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain.Drawing
{
    public static class ViewModelMapper
    {
        private const string PowerUpText = "POWER UP";
        private static readonly Color PowerUpTextColor = Color.Black;

        public static GameViewModel CreateViewModel(LobGame lobGame)
        {
            var state = new GameViewModel();

            state.CameraPosition = lobGame.GetCameraPosition();

            MapShrubbery(lobGame, state);
            MapExplosionFragments(lobGame, state);
            MapPlayer(lobGame, state);
            MapBullets(lobGame, state);
            MapEnemies(lobGame, state);
            MapSplashes(lobGame, state);
            MapPowerUpText(lobGame, state);

            return state;
        }

        private static void MapSplashes(LobGame lobGame, GameViewModel state)
        {
            foreach (var splash in lobGame.GetCollisionSplashes())
            {
                state.Entities.Add(new EntityViewModel()
                {
                    Type = EntityType.Splash,
                    Position = splash.Position
                });
            }
        }

        private static void MapEnemies(LobGame lobGame, GameViewModel state)
        {
            foreach (var enemy in lobGame.GetEnemies())
            {
                state.Entities.Add(new EntityViewModel()
                {
                    Type = EntityType.Enemy,
                    Position = enemy.Position,
                    Rotation = enemy.Direction,
                    HasRotation = true
                });
            }
        }

        private static void MapBullets(LobGame lobGame, GameViewModel state)
        {
            foreach (var bullet in lobGame.GetBullets())
            {
                state.Entities.Add(new EntityViewModel()
                {
                    Type = EntityType.Bullet,
                    Position = bullet.Position
                });
            }
        }

        private static void MapPlayer(LobGame lobGame, GameViewModel state)
        {
            state.Entities.Add(new EntityViewModel()
            {
                Type = EntityType.Player,
                Position = lobGame.GetPlayerPosition().ToVector2(),
                Rotation = lobGame.GetPlayerFacingDirection(),
                HasRotation = true,
            });
        }

        private static void MapExplosionFragments(LobGame lobGame, GameViewModel state)
        {
            foreach (var fragment in lobGame.GetFragments())
            {
                state.Entities.Add(new EntityViewModel()
                {
                    Type = EntityType.ExplosionFragment,
                    Position = fragment.Position
                });
            }
        }

        private static void MapShrubbery(LobGame lobGame, GameViewModel state)
        {
            foreach (var shrubbery in lobGame.GetShrubbery())
            {
                state.Entities.Add(new EntityViewModel()
                {
                    Type = EntityType.Shrubbery,
                    Position = shrubbery.Position.ToVector2()
                });
            }
        }

        private static void MapPowerUpText(LobGame lobGame, GameViewModel state)
        {
            if (lobGame.ShouldTriggerPowerUpText())
            {
                state.Text = new TextViewModel()
                {
                    ShouldShowText = true,
                    Position = CalculateTextPosition(lobGame.GetPlayerPosition()),
                    Text = PowerUpText,
                    Color = PowerUpTextColor
                };
            }
            else
            {
                state.Text = new TextViewModel()
                {
                    ShouldShowText = false
                };
            }
        }

        private static Vector2 CalculateTextPosition(Point playerPosition)
        {
            return (playerPosition - new Point(50, -20)).ToVector2();
        }
    }

    public enum EntityType
    {
        Shrubbery,
        ExplosionFragment,
        Bullet,
        Enemy,
        Player,
        Splash
    }
}