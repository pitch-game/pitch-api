using Pitch.Card.Api.Application.Responses;
using Pitch.Card.Api.Infrastructure.Requests;
using System.Threading.Tasks;

namespace Pitch.Card.Api.Infrastructure.Handlers
{
    public interface ICreateCardResponder
    {
        Task<CreateCardResponse> Response(CreateCardRequest @request);
    }
}