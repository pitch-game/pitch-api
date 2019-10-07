using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Providers
{
    public interface IRandomnessProvider
    {
        int Next(int minValue, int maxValue);
    }

    public class RandomnessProvider : IRandomnessProvider
    {
        private readonly Random _random;

        public RandomnessProvider()
        {
            _random = new Random();
        }

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
