using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Core.Domain.Drawing
{
    public class GameViewModel
    {
        public GameViewModel()
        {
            Entities = new List<EntityViewModel>();
        }

        public Point CameraPosition { get; set; }
        public TextViewModel Text { get; set; }
        public List<EntityViewModel> Entities { get; set; }
    }
}