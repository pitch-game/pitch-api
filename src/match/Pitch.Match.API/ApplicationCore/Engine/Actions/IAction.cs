using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.ApplicationCore.Models;
using System;
using System.Collections.Generic;

namespace Pitch.Match.API.ApplicationCore.Engine.Actions
{
    public interface IAction
    {
        decimal ChancePerMinute { get; }
        IDictionary<PositionalArea, decimal> PositionalChance { get; }
        bool AffectsTeamInPossession { get; }
        IEvent SpawnEvent(Card card, Guid squadId, Models.Match match);
    }
}
