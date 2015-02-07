using System;
using Microsoft.Xna.Framework;

namespace MonogameDemoGame.Services
{
    public class BoundaryService : IBoundaryService
    {
        private const int GameBorder = 2000;

        private readonly IRandomNumberService _randomNumberService;

        public BoundaryService(IRandomNumberService randomNumberService)
        {
            _randomNumberService = randomNumberService;
        }

        public bool OutOfBounds(float position)
        {
            return Math.Abs(position) > GameBorder;
        }

        public Point CreatePointInBoundary()
        {
            return new Point(
                _randomNumberService.NextRandomNumberBetweenPositiveAndNegative(GameBorder), 
                _randomNumberService.NextRandomNumberBetweenPositiveAndNegative(GameBorder));
        }
    }
}