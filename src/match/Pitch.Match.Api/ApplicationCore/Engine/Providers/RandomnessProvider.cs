using System;

namespace Pitch.Match.Api.ApplicationCore.Engine.Providers
{
    public interface IRandomnessProvider
    {
        int Next(int minValue, int maxValue);
    }

    public class RandomnessProvider : IRandomnessProvider
    {
        private Random random;

        public RandomnessProvider()
        {
            random = new Random();
        }

        public int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
    }
}
