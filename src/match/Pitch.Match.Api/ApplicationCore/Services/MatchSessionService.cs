using System.Collections.Generic;
using Pitch.Match.API.ApplicationCore.Models.Matchmaking;

namespace Pitch.Match.API.ApplicationCore.Services
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
