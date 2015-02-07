using System;

namespace MonogameDemoGame.Services
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly Random _random;

        public RandomNumberService()
        {
            _random = new Random();
        }

        public RandomNumberService(int seed)
        {
            _random = new Random(seed);
        }

        public int GenerateRandomNegativeOrPositiveOne()
        {
            return GetRandomBool() ? 1 : -1;
        }

        public int NextRandomNumber(int maxValue)
        {
            return NextRandomNumber(0, maxValue);
        }

        public int NextRandomNumberBetweenPositiveAndNegative(int value)
        {
            return NextRandomNumber(value);
        }

        public bool GetRandomBool()
        {
            return NextRandomNumber(1) == 1;
        }

        public int NextRandomNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }

        public double GenerateRandomNumberClusteredTowardZero(int max)
        {
            return Math.Sqrt(NextRandomNumber(max * max));
        }
    }
}