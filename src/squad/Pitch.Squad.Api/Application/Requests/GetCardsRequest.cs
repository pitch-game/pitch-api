using System;
using System.Collections.Generic;

namespace Pitch.Squad.Api.Application.Requests
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
