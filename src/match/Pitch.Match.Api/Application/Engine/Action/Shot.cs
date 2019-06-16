using Pitch.Match.Api.Models;
using System.Collections.Generic;


namespace Pitch.Match.Api.Application.Engine.Action
{
    public class Shot : IAction
    {
        public decimal ChancePerMinute => 0.07m;

        public IDictionary<PositionalArea, decimal> PositionalChance => new Dictionary<PositionalArea, decimal>()
        {
            { PositionalArea.GK, 0.0m },
            { PositionalArea.DEF, 0.10m },
            { PositionalArea.MID, 0.40m },
            { PositionalArea.ATT, 0.50m },
        };

        public bool AffectsTeamInPossession => true;
    }
}
