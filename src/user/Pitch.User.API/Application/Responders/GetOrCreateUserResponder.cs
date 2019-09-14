using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.User.API.Application.Requests;
using Pitch.User.API.Application.Responses;
using Pitch.User.API.Services;
using System.Threading.Tasks;

namespace Pitch.User.API.Application.Responders
{
    public interface IGetOrCreateUserResponder
    {
        Task<GetOrCreateUserResponse> Response(GetOrCreateUserRequest request);
    }

    public class GetOrCreateUserResponder : IGetOrCreateUserResponder, IResponder
    {
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetOrCreateUserResponder(IBus bus, IServiceScopeFactory serviceScopeFactory)
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Register()
        {
            _bus.RespondAsync<GetOrCreateUserRequest, GetOrCreateUserResponse>(Response);
        }

        public async Task<GetOrCreateUserResponse> Response(GetOrCreateUserRequest @request)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var user = await scope.ServiceProvider.GetRequiredService<IUserService>().GetOrCreateAsync(@request.Email);
                return new GetOrCreateUserResponse()
                {
                    Id = user.Id
                };
            }
        }
    }
}
