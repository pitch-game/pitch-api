using Pitch.Card.Api.Application.Requests;
using Pitch.Card.Api.Application.Responses;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Application.Responders
{
    public interface ICreateCardResponder
    {
        Task<CreateCardResponse> Response(CreateCardRequest @request);
    }
}