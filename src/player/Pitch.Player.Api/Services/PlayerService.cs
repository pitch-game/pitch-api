using Pitch.Player.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pitch.Player.Api.Services
{
    public interface IPlayerService
    {
        Models.Player GetRandom(RandomPlayerRequestModel req);
    }
    public class PlayerService : IPlayerService
    {
        private readonly IList<Models.Player> _players;

        public PlayerService(IList<Models.Player> players)
        {
            _players = players;
        }

        public Models.Player GetRandom(RandomPlayerRequestModel req)
        {
            var filter = _players.Where(x => x.Rating >= req.RatingRange.lower && x.Rating <= req.RatingRange.upper && x.Positions.Contains(req.Position));

            //todo weighting based on x.Rating
            return new Models.Player();
        }
    }
}
