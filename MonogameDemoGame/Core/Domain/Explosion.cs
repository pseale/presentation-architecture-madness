using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain
{
    public class Explosion
    {
        private const int ExplosionTicks = 120;

        private int _ticks;

        public Explosion(Vector2 position, IEnumerable<Vector2> fragments)
        {
            Position = position;
            Fragments = new List<ExplosionFragment>(fragments.Select(x => new ExplosionFragment(Position + x, x)));
        }

        public Vector2 Position { get; private set; }
        public List<ExplosionFragment> Fragments { get; private set; }

        public ExplosionUpdateResult Update()
        {
            _ticks++;

            if (_ticks > ExplosionTicks)
                return ExplosionUpdateResult.Remove;

            foreach (var fragment in Fragments)
                fragment.Update();

            return ExplosionUpdateResult.DoNothing;
        }
    }
}