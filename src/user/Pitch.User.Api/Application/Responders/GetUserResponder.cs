using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.User.Api.Application.Requests;
using Pitch.User.Api.Application.Responses;
using Pitch.User.Api.Services;
using System.Threading.Tasks;

namespace Pitch.User.Api.Application.Responders
{
    public interface IGetUserResponder
    {
        Task<GetUserResponse> Response(GetUserRequest request);
    }

    public class GetUserResponder : IGetUserResponder, IResponder
    {
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetUserResponder(IBus bus, IServiceScopeFactory serviceScopeFactory)
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Register()
        {
            _bus.RespondAsync<GetUserRequest, GetUserResponse>(Response);
        }

        public async Task<GetUserResponse> Response(GetUserRequest @request)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var user = await scope.ServiceProvider.GetRequiredService<IUserService>().GetAsync(@request.Email);
                return new GetUserResponse();
            }
        }
    }
}
