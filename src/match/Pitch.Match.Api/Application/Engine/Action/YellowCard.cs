using Pitch.Match.Api.Models;
using System.Collections.Generic;


namespace Pitch.Match.Api.Application.Engine.Action
{
    public class YellowCard : IAction
    {
        public decimal ChancePerMinute => 0.01m;

        public IDictionary<PositionalArea, decimal> PositionalChance => new Dictionary<PositionalArea, decimal>()
        {
            { PositionalArea.GK, 0.05m },
            { PositionalArea.DEF, 0.40m },
            { PositionalArea.MID, 0.40m },
            { PositionalArea.ATT, 0.15m },
        };
    }
}
