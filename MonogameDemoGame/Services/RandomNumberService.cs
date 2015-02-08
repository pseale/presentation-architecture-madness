using System;

namespace MonogameDemoGame.Services
{
    public class RandomNumberService
    {
        private static readonly Random _random = new Random();

        public static int GenerateRandomNegativeOrPositiveOne()
        {
            return GetRandomBool() ? 1 : -1;
        }

        public static int NextRandomNumber(int maxValue)
        {
            return NextRandomNumber(0, maxValue);
        }

        public static int NextRandomNumberBetweenPositiveAndNegative(int value)
        {
            return NextRandomNumber(-value, value);
        }

        public static bool GetRandomBool()
        {
            return NextRandomNumber(1) == 1;
        }

        public static double GenerateRandomNumberClusteredTowardZero(int max)
        {
            return Math.Sqrt(NextRandomNumber(max * max));
        }

        public static int NextRandomNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }
    }
}