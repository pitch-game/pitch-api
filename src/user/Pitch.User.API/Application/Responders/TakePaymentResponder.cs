using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Pitch.User.API.Application.Requests;
using Pitch.User.API.Application.Responses;
using Pitch.User.API.Services;
using System.Threading.Tasks;

namespace Pitch.User.API.Application.Responders
{
    public interface ITakePaymentResponder
    {
        Task<TakePaymentResponse> Response(TakePaymentRequest request);
    }

    public class TakePaymentResponder : ITakePaymentResponder, IResponder
    {
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TakePaymentResponder(IBus bus, IServiceScopeFactory serviceScopeFactory)
        {
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Register()
        {
            _bus.RespondAsync<TakePaymentRequest, TakePaymentResponse>(Response);
        }

        public async Task<TakePaymentResponse> Response(TakePaymentRequest @request)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var success = await scope.ServiceProvider.GetRequiredService<IUserService>().TakePayment(@request.UserId, @request.Amount);
                return new TakePaymentResponse()
                {
                    Success = success
                };
            }
        }
    }
}