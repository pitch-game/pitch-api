﻿using System;
using System.Collections.Generic;

namespace Pitch.Match.Engine.Services
{
    //TODO Refactor as service and use DI
    public static class ChanceHelper
    {
        public static T PercentBase100Chance<T>(IEnumerable<T> inputs, Func<T, decimal> field)
        {
            Random random = new Random(); //TODO use randomness provider
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

            var rand = new Random();  //TODO use randomness provider
            var randomNumber = rand.Next(0, accumulatedWeight);
            if (randomNumber <= chanceOfTrue)
            {
                return true;
            }
            return false;
        }
    }
}
