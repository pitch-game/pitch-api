using Pitch.Match.Api.Models;
using System.Collections.Generic;

namespace Pitch.Match.Api.Application.MessageBus.Responses
{
    public class GetSquadResponse
    {
        public IList<Card> Cards { get; set; }
    }
}
