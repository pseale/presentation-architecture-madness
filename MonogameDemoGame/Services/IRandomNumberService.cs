using System;

namespace MonogameDemoGame.Services
{
    public interface IRandomNumberService
    {
        int GenerateRandomNegativeOrPositiveOne();
        int NextRandomNumber(int maxValue);
        int NextRandomNumberBetweenPositiveAndNegative(int value);
        bool GetRandomBool();
        int NextRandomNumber(int minValue, int maxValue);
        double GenerateRandomNumberClusteredTowardZero(int max);
    }
}