using System.Collections.Generic;
using Pitch.Match.Api.ApplicationCore.Models.Matchmaking;

namespace Pitch.Match.Api.ApplicationCore.Services
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
