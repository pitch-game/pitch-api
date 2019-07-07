using MongoDB.Bson.Serialization.Attributes;
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

        public Squad GetSquad(Guid id)
        {
            return HomeTeam.Squad.Id == id ? HomeTeam.Squad : AwayTeam.Squad.Id == id ? AwayTeam.Squad : throw new Exception();
        }

        public Squad GetOppositionSquad(Guid id)
        {
            return HomeTeam.Squad.Id == id ? AwayTeam.Squad : AwayTeam.Squad.Id == id ? HomeTeam.Squad : throw new Exception();
        }

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
        public MinuteStats(int minute, Guid squadIdInPossession, int homePossChance, int awayPossChance)
        {
            Minute = minute;
            SquadIdInPossession = squadIdInPossession;
            HomePossessionChance = homePossChance;
            AwayPossessionChance = awayPossChance;
        }
        public int Minute { get; set; }
        public Guid SquadIdInPossession { get; set; }

        public int HomePossessionChance { get; set; }
        public int AwayPossessionChance { get; set; }

    }
}
