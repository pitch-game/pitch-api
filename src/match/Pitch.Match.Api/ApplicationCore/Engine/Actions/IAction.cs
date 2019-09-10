using Pitch.Match.Api.ApplicationCore.Engine.Events;
using Pitch.Match.Api.ApplicationCore.Models;
using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.ApplicationCore.Engine.Actions
{
    public interface IAction
    {
        decimal ChancePerMinute { get; }
        IDictionary<PositionalArea, decimal> PositionalChance { get; }
        bool AffectsTeamInPossession { get; }
        IEvent SpawnEvent(Card card, Guid squadId, int minute, Models.Match match);
    }
}
