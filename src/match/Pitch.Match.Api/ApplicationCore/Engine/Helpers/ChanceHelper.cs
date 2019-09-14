using System;
using System.Collections.Generic;

namespace Pitch.Match.API.ApplicationCore.Engine.Helpers
{
    public static class ChanceHelper
    {
        public static T PercentBase100Chance<T>(IEnumerable<T> inputs, Func<T, decimal> field)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            var accumulatedProbability = 0m;
            foreach (var input in inputs)
            {
                accumulatedProbability += field(input);
                if (randomNumber <= accumulatedProbability * 100)
                {
                    return input;
                }
            }

            return default;
        }

        public static bool CumulativeTrueOrFalse(int chanceOfTrue, int chanceOfFalse)
        {
            var accumulatedWeight = chanceOfTrue + chanceOfFalse;

            var rand = new Random();
            var randomNumber = rand.Next(0, accumulatedWeight);
            if (randomNumber <= chanceOfTrue)
            {
                return true;
            }
            return false;
        }
    }
}
