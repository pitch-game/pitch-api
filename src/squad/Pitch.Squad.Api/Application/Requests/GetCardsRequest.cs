using System;
using System.Collections.Generic;

namespace Pitch.Squad.Api.Application.Requests
{
    public class GetCardsRequest
    {
        public GetCardsRequest(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
        public IEnumerable<Guid> Ids { get; set; }
    }
}
