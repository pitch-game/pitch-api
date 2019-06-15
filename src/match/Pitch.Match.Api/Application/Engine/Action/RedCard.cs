using Pitch.Match.Api.Models;
using System.Collections.Generic;

namespace Pitch.Match.Api.Application.Engine.Action
{
    public class RedCard : IAction
    {
        public decimal ChancePerMinute => 0.005m;
        public IDictionary<PositionalArea, decimal> PositionalChance => new Dictionary<PositionalArea, decimal>()
        {
            { PositionalArea.GK, 0.25m },
            { PositionalArea.DEF, 0.40m },
            { PositionalArea.MID, 0.25m },
            { PositionalArea.ATT, 0.10m },
        };
    }
}
