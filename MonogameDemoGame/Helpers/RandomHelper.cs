using System;

namespace MonogameDemoGame.Helpers
{
    public static class RandomHelper
    {
        public static int GenerateRandomNegativeOrPositiveOne(Random random)
        {
            return GetRandomBool(random) ? 1 : -1;
        }

        public static int NextRandomNumber(Random random, int maxValue)
        {
            return NextRandomNumber(random, 0, maxValue);
        }

        public static int NextRandomNumberBetweenPositiveAndNegative(Random random, int value)
        {
            return NextRandomNumber(random, -value, value);
        }

        public static bool GetRandomBool(Random random)
        {
            return NextRandomNumber(random, 1) == 1;
        }

        public static int NextRandomNumber(Random random, int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }

        public static double GenerateRandomNumberClusteredTowardZero(Random random, int max)
        {
            return Math.Sqrt(NextRandomNumber(random, max * max));
        }
    }
}