using System;
using System.Collections.Generic;

namespace Pitch.Card.Api.Application.Requests
{
    public class GetCardsRequest
    {
        public GetCardsRequest(IList<Guid> ids)
        {
            Ids = ids;
        }
        public IList<Guid> Ids { get; set; }
    }
}
