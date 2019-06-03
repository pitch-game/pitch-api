using System;
using System.Collections.Generic;

namespace Pitch.Card.Api.Models
{
    public class CardRequestModel
    {
        public CardRequestModel()
        {
            NotIn = new List<Guid>();
        }

        public int Skip { get; set; }
        public int Take { get; set; }
        public string PositionPriority { get; set; }
        public IEnumerable<Guid> NotIn { get; set; }
    }
}
