namespace MonogameDemoGame.Core.Domain.Spawning
{
    public class InitialGameState
    {
        public CameraDto Camera { get; set; }
        public PlayerDto Player { get; set; }
        public ShrubberyDto[] Shrubbery { get; set; }
        public EnemyDto[] Enemies { get; set; }
        public int GameBorder { get; set; }
    }
}