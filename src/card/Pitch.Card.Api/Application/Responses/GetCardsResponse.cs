using System.Collections.Generic;

namespace Pitch.Card.API.Application.Responses
{
    public class GetCardsResponse
    {
        public IEnumerable<Models.Card> Cards { get; set; }
    }
}
