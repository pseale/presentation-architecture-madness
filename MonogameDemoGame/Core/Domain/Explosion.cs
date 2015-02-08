using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonogameDemoGame.Structs;

namespace MonogameDemoGame.Core.Domain
{
    public class Explosion
    {
        private const int ExplosionTicks = 120;

        private int _ticks;

        public Explosion(Vector2 position, IEnumerable<Vector2> fragments)
        {
            Position = position;
            Fragments = new List<ExplosionFragment>(fragments.Select(x => new ExplosionFragment(x)));
        }

        public Vector2 Position { get; private set; }
        public List<ExplosionFragment> Fragments { get; private set; }
        public int Ticks { get { return _ticks; } }

        public ExplosionUpdateResult Update()
        {
            _ticks++;

            if (_ticks > ExplosionTicks)
                return ExplosionUpdateResult.Remove;

            return ExplosionUpdateResult.DoNothing;
        }
    }
}