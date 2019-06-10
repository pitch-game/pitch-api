using System.Collections.Generic;

namespace Pitch.Card.Api.Application.Responses
{
    public class GetCardsResponse
    {
        public IEnumerable<Models.Card> Cards { get; set; }
    }
}
