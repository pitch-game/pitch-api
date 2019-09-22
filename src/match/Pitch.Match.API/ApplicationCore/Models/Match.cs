using Pitch.Match.API.ApplicationCore.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.Infrastructure.Repositories;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.API.ApplicationCore.Models
{
    public class Match : IEntity
    {
        public Match()
        {
            Events = new List<IEvent>();
            Statistics = new List<MinuteStats>();
        }
        public Guid Id { get; set; }

        public virtual TeamDetails HomeTeam { get; set; }
        public virtual TeamDetails AwayTeam { get; set; }

        public Squad GetSquad(Guid id)
        {
            return HomeTeam.Squad.Id == id ? HomeTeam.Squad : AwayTeam.Squad.Id == id ? AwayTeam.Squad : throw new Exception();
        }

        public Squad GetOppositionSquad(Guid id)
        {
            return HomeTeam.Squad.Id == id ? AwayTeam.Squad : AwayTeam.Squad.Id == id ? HomeTeam.Squad : throw new Exception();
        }

        public TeamDetails GetTeam(Guid userId)
        {
            return HomeTeam.UserId == userId ? HomeTeam : AwayTeam.UserId == userId ? AwayTeam : throw new Exception();
        }

        public DateTime KickOff { get; set; }

        public IList<IEvent> Events { get; set; }
        public IList<MinuteStats> Statistics { get; set; }

        public int ExtraTime { get; set; }

        public int Duration => (int)DateTime.Now.Subtract(KickOff).TotalMinutes;

        public bool IsOver => DateTime.Now > KickOff.AddMinutes(90 + ExtraTime);

        public void AsOfNow()
        {
            Events = Events.Where(x => x.Minute < Duration).ToList();
            Statistics = Statistics.Where(x => x.Minute < Duration).ToList();
        }

        public virtual void Substitute(Guid off, Guid on, Guid userId)
        {
            var team = GetTeam(userId);
            team.Squad.Substitute(off, on);

            Events.Add(new Substitution(Duration, on, team.Squad.Id));
        }
    }

    public class TeamDetails
    {
        public Guid UserId { get; set; }
        public Squad Squad { get; set; }
        public bool HasClaimedRewards { get; set; }
        public virtual int UsedSubs { get; set; }
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
