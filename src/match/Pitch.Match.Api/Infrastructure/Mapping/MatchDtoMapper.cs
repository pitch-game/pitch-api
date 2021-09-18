using System.Collections.Generic;
using System.Linq;
using Pitch.Match.Api.Infrastructure.Models;

namespace Pitch.Match.Api.Infrastructure.Mapping
{
    public static class MatchMapper
    {
        public static Models.Match Map(Engine.Models.Match match)
        {
            return new Models.Match
            {
                Id = match.Id,
                AwayTeam = MapTeamDetails(match.AwayTeam),
                HomeTeam = MapTeamDetails(match.HomeTeam),
                KickOff = match.KickOff,
                Minutes = MapMinutes(match.Minutes),
                Version = match.Version
            };
        }

        private static TeamDetails MapTeamDetails(Engine.Models.TeamDetails teamDetails)
        {
            return new TeamDetails
            {
                UserId = teamDetails.UserId,
                HasClaimedRewards = teamDetails.HasClaimedRewards,
                Squad = MapSquad(teamDetails.Squad),
                UsedSubs = teamDetails.UsedSubs
            };
        }

        private static Squad MapSquad(Engine.Models.Squad squad)
        {
            return new Squad
            {
                Id = squad.Id,
                Lineup = MapLineup(squad.Lineup),
                Name = squad.Name,
                Subs = MapCards(squad.Subs)
            };
        }

        private static Card[] MapCards(IEnumerable<Engine.Models.Card> cards)
        {
            return cards?.Select(card => new Card
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

        private static IDictionary<string, IEnumerable<Card>> MapLineup(IDictionary<string, IEnumerable<Engine.Models.Card>> lineup)
        {
            return lineup.ToDictionary(x => x.Key, x => (IEnumerable<Card>)MapCards(x.Value));
        }

        private static MatchMinute[] MapMinutes(Engine.Models.MatchMinute[] matchMinutes)
        {
            return matchMinutes.Select(x => new MatchMinute()
            {
                Stats = MapMinuteStats(x.Stats),
                Modifiers = MapModifiers(x.Modifiers),
                Events = MapEvents(x.Events)
            }).ToArray();
        }

        private static MinuteStats MapMinuteStats(Engine.Models.MinuteStats minuteStats)
        {
            if (minuteStats == null)
            {
                return null;
            }

            return new MinuteStats
            {
                AwayPossessionChance = minuteStats.AwayPossessionChance,
                HomePossessionChance = minuteStats.HomePossessionChance,
                SquadIdInPossession = minuteStats.SquadIdInPossession
            };
        }

        private static IList<Modifier> MapModifiers(IEnumerable<Engine.Models.Modifier> modifiers)
        {
            return modifiers.Select(modifier => new Modifier
            {
                CardId = modifier.CardId,
                DrainValue = modifier.DrainValue,
                Type = (ModifierType)modifier.Type
            }).ToList();
        }

        private static IList<Event> MapEvents(IEnumerable<Engine.Models.Event> events)
        {
            return events?.Select(@event => new Event
            {
                SquadId = @event.SquadId,
                CardId = @event.CardId,
                Type = (EventType) @event.Type
            }).ToList();
        }
    }
}
