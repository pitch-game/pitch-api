using Pitch.Match.Api.Models.Matchmaking;
using System.Collections.Generic;

namespace Pitch.Match.Api.Services
{
    public interface IMatchSessionService
    {
        IList<MatchmakingSession> Sessions { get; set; }
    }

    public class MatchSessionService : IMatchSessionService
    {
        public MatchSessionService()
        {
            Sessions = new List<MatchmakingSession>();
        }
        public IList<MatchmakingSession> Sessions { get; set; }
    }
}
