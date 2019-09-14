using Pitch.Squad.API.Application.Models;
using System.Collections.Generic;

namespace Pitch.Squad.API.Application.Response
{
    public class GetCardsResponse
    {
        public IEnumerable<CardDTO> Cards { get; set; }
    }
}
