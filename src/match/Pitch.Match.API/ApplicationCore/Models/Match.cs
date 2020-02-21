using Pitch.Match.API.ApplicationCore.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.API.ApplicationCore.Models
{
    public class Match : IEntity
    {
        public Match()
        {
            Minutes = Enumerable.Range(0, Constants.MatchLengthInMinutes).Select( i=> new MatchMinute()).ToArray();
        }

        public Guid Id { get; set; }

        public virtual TeamDetails HomeTeam { get; set; }
        public virtual TeamDetails AwayTeam { get; set; }

        public DateTime KickOff { get; set; }

        public MatchMinute[] Minutes { get; set; }
        
        /// <summary>
        /// The current elapsed minutes
        /// </summary>
        public int Elapsed => (int)DateTime.Now.Subtract(KickOff).TotalMinutes;

        public bool HasFinished => DateTime.Now > KickOff.AddMinutes(Constants.MatchLengthInMinutes);

        public void AsAtElapsed(bool includeCurrentMinute = false)
        {
            var elapsed = includeCurrentMinute ? Elapsed + 1 : Elapsed;
            //Reset match minutes
            for (int i = elapsed; i < Constants.MatchLengthInMinutes; i++)
            {
                Minutes[i] = new MatchMinute();
            }
        }

        public virtual void Substitute(Guid off, Guid on, Guid userId)
        {
            var team = GetTeam(userId);
            team.Squad.Substitute(off, on);
            Minutes[Elapsed].Events.Add(new Substitution(on, team.Squad.Id));
        }

        public Squad GetSquad(Guid id)
        {
            return HomeTeam.Squad.Id == id ? HomeTeam.Squad : AwayTeam.Squad.Id == id ? AwayTeam.Squad : null;
        }

        public Squad GetOppositionSquad(Guid id)
        {
            return HomeTeam.Squad.Id == id ? AwayTeam.Squad : AwayTeam.Squad.Id == id ? HomeTeam.Squad : null;
        }

        public TeamDetails GetTeam(Guid userId)
        {
            return HomeTeam.UserId == userId ? HomeTeam : AwayTeam.UserId == userId ? AwayTeam : null;
        }
    }

    public class TeamDetails
    {
        public Guid UserId { get; set; }
        public Squad Squad { get; set; }
        public bool HasClaimedRewards { get; set; }
        public virtual int UsedSubs { get; set; }
    }

    public class Modifier
    {
        public Guid CardId { get; set; }
        public float DrainValue { get; set; }
        public ModifierType Type { get; set; }
    }

    public enum ModifierType
    {
        Fitness
    }

    public class MatchMinute
    {
        public MatchMinute()
        {
            Modifiers = new List<Modifier>();
            Events = new List<IEvent>();
        }

        public MinuteStats Stats { get; set; }
        public IList<Modifier> Modifiers { get; set; }
        public IList<IEvent> Events { get; set; }
    }

    public class MinuteStats
    {
        public MinuteStats(Guid squadIdInPossession, int homePossChance, int awayPossChance)
        {
            SquadIdInPossession = squadIdInPossession;
            HomePossessionChance = homePossChance;
            AwayPossessionChance = awayPossChance;
        }

        public Guid SquadIdInPossession { get; set; }
        public int HomePossessionChance { get; set; }
        public int AwayPossessionChance { get; set; }
    }
}
