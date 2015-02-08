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

            foreach (var shrubbery in lobGame.GetShrubbery())
                state.Entities.Add(CreateEntity(EntityType.Shrubbery, shrubbery.Position.ToVector2()));

            foreach (var fragment in lobGame.GetFragments())
                state.Entities.Add(CreateEntity(EntityType.ExplosionFragment, fragment.Position));

            state.Entities.Add(CreateEntity(EntityType.Player, lobGame.GetPlayerPosition().ToVector2(), lobGame.GetPlayerFacingDirection()));

            foreach (var bullet in lobGame.GetBullets())
                state.Entities.Add(CreateEntity(EntityType.Bullet, bullet.Position));

            foreach (var enemy in lobGame.GetEnemies())
                state.Entities.Add(CreateEntity(EntityType.Enemy, enemy.Position, enemy.Direction));

            foreach (var splash in lobGame.GetCollisionSplashes())
                state.Entities.Add(CreateEntity(EntityType.Splash, splash.Position));
            
            MapPowerUpText(lobGame, state);

            return state;
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

        private static EntityViewModel CreateEntity(EntityType entityType, Vector2 position)
        {
            return new EntityViewModel()
            {
                Type = entityType,
                Position = position
            };
        }

        private static EntityViewModel CreateEntity(EntityType entityType, Vector2 position, Vector2 rotation)
        {
            return new EntityViewModel()
            {
                Type = entityType,
                Position = position,
                Rotation = rotation,
                HasRotation = true
            };
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