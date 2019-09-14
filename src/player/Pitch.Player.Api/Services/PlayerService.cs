using Pitch.Player.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Player.API.Services
{
    public interface IPlayerService
    {
        Models.Player GetRandom(PlayerRequestModel req);
    }
    public class PlayerService : IPlayerService
    {
        private readonly IList<Models.Player> _players;

        public PlayerService(IList<Models.Player> players)
        {
            _players = players;
        }

        public Models.Player GetRandom(PlayerRequestModel req)
        {
            IEnumerable<Models.Player> players = _players;

            if (req.Position != null)
                players = players.Where(x => x.Positions.Contains(req.Position));
            if (req.RatingRange.HasValue && req.RatingRange.Value.lower != null)
                players = players.Where(x => x.Rating >= req.RatingRange.Value.lower);
            if (req.RatingRange.HasValue && req.RatingRange.Value.upper != null)
                players = players.Where(x => x.Rating <= req.RatingRange.Value.upper);

            return GetRandomPlayerWeightedByRating(players);
        }

        private const int RATING_OFFSET = 100;
        private static Models.Player GetRandomPlayerWeightedByRating(IEnumerable<Models.Player> players)
        {
            var totalWeight = players.Sum(x => RATING_OFFSET - x.Rating);
            var random = new Random().Next(totalWeight);

            foreach (var player in players)
            {
                random = random - (RATING_OFFSET - player.Rating);
                if (random <= 0)
                    return player;
            }
            throw new IndexOutOfRangeException();
        }
    }
}
