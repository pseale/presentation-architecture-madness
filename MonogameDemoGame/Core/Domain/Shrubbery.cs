using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain
{
    public class Shrubbery
    {
        public Shrubbery(Point position)
        {
            Position = position;
        }

        public Point Position { get; private set; } 
    }
}