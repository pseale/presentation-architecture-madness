using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain
{
    public class ExplosionFragment
    {
        public ExplosionFragment(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; private set; }
    }
}