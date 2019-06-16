using Pitch.Match.Api.Models;
using System.Collections.Generic;

namespace Pitch.Match.Api.Application.Engine.Action
{
    public interface IAction
    {
        decimal ChancePerMinute { get; }
        IDictionary<PositionalArea, decimal> PositionalChance { get; }
        bool AffectsTeamInPossession { get; }
    }
}
