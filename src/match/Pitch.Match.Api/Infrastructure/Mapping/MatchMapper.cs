using System.Collections.Generic;
using System.Linq;
using Pitch.Match.Engine.Models;
using Card = Pitch.Match.Api.Infrastructure.Models.Card;
using MatchMinute = Pitch.Match.Api.Infrastructure.Models.MatchMinute;
using MinuteStats = Pitch.Match.Api.Infrastructure.Models.MinuteStats;
using Modifier = Pitch.Match.Api.Infrastructure.Models.Modifier;
using Squad = Pitch.Match.Api.Infrastructure.Models.Squad;
using TeamDetails = Pitch.Match.Api.Infrastructure.Models.TeamDetails;

namespace Pitch.Match.Api.Infrastructure.Mapping
{
    public interface IMatchDtoMapper
    {
        Engine.Models.Match Map(Models.Match match);
    }

    public class MatchDtoMapper : IMatchDtoMapper
    {
        public Engine.Models.Match Map(Models.Match match)
        {
            return new Engine.Models.Match
            {
                Id = match.Id,
                AwayTeam = MapTeamDetails(match.AwayTeam),
                HomeTeam = MapTeamDetails(match.HomeTeam),
                KickOff = match.KickOff,
                Minutes = MapMinutes(match.Minutes),
                Version = match.Version
            };
        }

        private static Engine.Models.TeamDetails MapTeamDetails(TeamDetails teamDetails)
        {
            if (teamDetails == null)
            {
                return null;
            }

            return new Engine.Models.TeamDetails
            {
                UserId = teamDetails.UserId,
                HasClaimedRewards = teamDetails.HasClaimedRewards,
                Squad = MapSquad(teamDetails.Squad),
                UsedSubs = teamDetails.UsedSubs
            };
        }

        private static Engine.Models.Squad MapSquad(Squad squad)
        {
            return new Engine.Models.Squad
            {
                Id = squad.Id,
                Lineup = MapLineup(squad.Lineup),
                Name = squad.Name,
                Subs = MapCards(squad.Subs)
            };
        }

        private static Engine.Models.Card[] MapCards(IEnumerable<Card> cards)
        {
            return cards?.Select(card => new Engine.Models.Card
            {
                Id = card.Id,
                Chemistry = card.Chemistry,
                Fitness = card.Fitness,
                Goals = card.Goals,
                Name = card.Name,
                Position = card.Position,
                Rarity = card.Rarity,
                Rating = card.Rating,
                RedCards = card.RedCards,
                ShortName = card.ShortName,
                YellowCards = card.YellowCards
            }).ToArray();
        }

        private static IDictionary<string, IEnumerable<Engine.Models.Card>> MapLineup(IDictionary<string, IEnumerable<Card>> lineup)
        {
            return lineup.ToDictionary(x => x.Key, x => (IEnumerable<Engine.Models.Card>)MapCards(x.Value));
        }

        private static Engine.Models.MatchMinute[] MapMinutes(MatchMinute[] matchMinutes)
        {
            return matchMinutes?.Select(x => new Engine.Models.MatchMinute()
            {
                Stats = MapMinuteStats(x.Stats),
                Modifiers = MapModifiers(x.Modifiers),
                Events = MapEvents(x.Events)
            }).ToArray();
        }

        private static Engine.Models.MinuteStats MapMinuteStats(MinuteStats minuteStats)
        {
            return new Engine.Models.MinuteStats(minuteStats.SquadIdInPossession, minuteStats.HomePossessionChance,
                minuteStats.AwayPossessionChance);
        }

        private static IList<Engine.Models.Modifier> MapModifiers(IEnumerable<Modifier> modifiers)
        {
            return modifiers?.Select(modifier => new Engine.Models.Modifier
            {
                CardId = modifier.CardId,
                DrainValue = modifier.DrainValue,
                Type = (ModifierType)modifier.Type
            }).ToList();
        }

        private static IList<Engine.Models.Event> MapEvents(IEnumerable<Infrastructure.Models.Event> events)
        {
            return events?.Select(@event => new Engine.Models.Event
            {
                SquadId = @event.SquadId,
                CardId = @event.CardId,
                Type = (Engine.Models.EventType)@event.Type
            }).ToList();
        }
    }
}
