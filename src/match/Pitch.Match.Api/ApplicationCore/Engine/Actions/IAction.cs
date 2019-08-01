using Pitch.Match.Api.ApplicationCore.Engine.Events;
using Pitch.Match.Api.ApplicationCore.Models.Match;
using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.ApplicationCore.Engine.Actions
{
    public interface IAction
    {
        decimal ChancePerMinute { get; }
        IDictionary<PositionalArea, decimal> PositionalChance { get; }
        bool AffectsTeamInPossession { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        /// <param name="squadId"></param>
        /// <param name="minute"></param>
        /// <param name="match"></param>
        /// <param name="forceReRoll"></param>
        /// <returns></returns>
        IEvent SpawnEvent(Card card, Guid squadId, int minute, Models.Match.Match match);
    }
}
