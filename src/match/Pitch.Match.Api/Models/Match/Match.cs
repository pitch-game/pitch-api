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
        public Guid HomeUserId { get; set; }
        public Guid AwayUserId { get; set; }

        public Squad HomeTeam { get; set; }
        public Squad AwayTeam { get; set; }

        public DateTime KickOff { get; set; }

        public IList<IEvent> Events { get; set; }
        public IList<MinuteStats> Statistics { get; set; }
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
