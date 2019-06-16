using Pitch.Squad.Api.Application.Models;
using System.Collections.Generic;

namespace Pitch.Squad.Api.Application.Response
{
    public class GetSquadResponse
    {
        public IList<Card> Cards { get; set; }
    }
}
