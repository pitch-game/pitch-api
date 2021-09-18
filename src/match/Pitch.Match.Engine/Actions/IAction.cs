using System;
using System.Collections.Generic;
using Pitch.Match.Engine.Models;

namespace Pitch.Match.Engine.Actions
{
    public interface IAction
    {
        decimal ChancePerMinute { get; }
        IDictionary<PositionalArea, decimal> PositionalChance { get; }
        bool AffectsTeamInPossession { get; }
        Event SpawnEvent(Card card, Guid squadId, Models.Match match);
    }
}
