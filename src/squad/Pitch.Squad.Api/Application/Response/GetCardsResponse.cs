using Pitch.Squad.Api.Application.Models;
using System.Collections.Generic;

namespace Pitch.Squad.Api.Application.Response
{
    public class GetCardsResponse
    {
        public IEnumerable<Card> Cards { get; set; }
    }
}
