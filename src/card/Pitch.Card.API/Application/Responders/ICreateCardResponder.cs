using Pitch.Card.API.Application.Requests;
using Pitch.Card.API.Application.Responses;
using System.Threading.Tasks;

namespace Pitch.Card.API.Application.Responders
{
    public interface ICreateCardResponder
    {
        Task<CreateCardResponse> Response(CreateCardRequest @request);
    }
}