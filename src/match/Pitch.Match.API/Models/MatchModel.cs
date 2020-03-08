using Pitch.Match.API.ApplicationCore.Models;
using Pitch.Match.API.ApplicationCore.Models.MatchResult;

namespace Pitch.Match.API.Models
{
    public class MatchModel
    {
        //TODO MatchResultModel
        public MatchResult Match { get; set; }
        public int SubsRemaining { get; set; }
    }
}
