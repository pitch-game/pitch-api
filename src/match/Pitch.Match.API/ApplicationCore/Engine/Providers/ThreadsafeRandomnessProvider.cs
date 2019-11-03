using System;

namespace Pitch.Match.API.ApplicationCore.Engine.Providers
{
    public interface IRandomnessProvider
    {
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
    }

    public class ThreadSafeRandomnessProvider : IRandomnessProvider
    {
        private static readonly Random Random = new Random();
        
        public int Next(int maxValue)
        {
            lock (Random)
            {
                return Random.Next(maxValue);
            }
        }

        public int Next(int minValue, int maxValue)
        {
            lock (Random)
            {
                return Random.Next(minValue, maxValue);
            }
        }
    }
}
