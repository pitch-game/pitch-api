using System;
using System.Linq;
using Pitch.Match.API.ApplicationCore.Engine;
using Pitch.Match.API.ApplicationCore.Engine.Events;
using Pitch.Match.API.Infrastructure.Repositories.Contexts;

namespace Pitch.Match.API.ApplicationCore.Models.Match
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
}
