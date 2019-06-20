using Pitch.Match.Api.Application.Engine.Events;
using System;
using System.Collections.Generic;

namespace Pitch.Match.Api.Models
{
    public class Match
    {
        public Match()
        {
            Events = new List<IEvent>();
            Statistics = new List<MinuteStats>();
        }
        public Guid Id { get; set; }

        public TeamDetails HomeTeam { get; set; }
        public TeamDetails AwayTeam { get; set; }

        public DateTime KickOff { get; set; }

        public IList<IEvent> Events { get; set; }
        public IList<MinuteStats> Statistics { get; set; }

        public int ExtraTime { get; set; }

        public int Duration => (int)DateTime.Now.Subtract(KickOff).TotalMinutes;

        public bool IsOver => DateTime.Now > KickOff.AddMinutes(90 + ExtraTime);
    }

    public class TeamDetails
    {
        public Guid UserId { get; set; }
        public Squad Squad { get; set; }
        public bool HasClaimedRewards { get; set; }
    }

    public class MinuteStats
    {
        public MinuteStats(int minute, Guid squadIdInPossession)
        {
            Minute = minute;
            SquadIdInPossession = squadIdInPossession;
        }
        public int Minute { get; set; }
        public Guid SquadIdInPossession { get; set; }
    }
}
