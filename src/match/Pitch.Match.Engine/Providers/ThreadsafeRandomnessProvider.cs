using System;

namespace Pitch.Match.Engine.Providers
{
    public interface IRandomnessProvider
    {
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }

    public class ThreadSafeRandomnessProvider : IRandomnessProvider
    {
        public int Next(int maxValue)
        {
            return Random.Shared.Next(maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            return Random.Shared.Next(minValue, maxValue);
        }
    }
}
