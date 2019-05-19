using System;
using System.Threading.Tasks;
using EasyNetQ;
using Pitch.Store.Api.Application.Requests;
using Pitch.Store.Api.Application.Responses;
using Pitch.Store.Api.Infrastructure.Repositories;

namespace Pitch.Store.Api.Infrastructure.Services
{
    public interface IPackService
    {
        Task<CreateCardResponse> Open(Guid id);
    }
    public class PackService : IPackService
    {
        private readonly IPackRepository _packRepository;
        private readonly IBus _bus;

        public PackService(IPackRepository packRepository, IBus bus)
        {
            _packRepository = packRepository;
            _bus = bus;
        }

        public async Task<CreateCardResponse> Open(Guid id)
        {
            var pack = await _packRepository.GetAsync(id);
            //check logged in userid matches card userid

            var request = new CreateCardRequest();
            return await _bus.RequestAsync<CreateCardRequest, CreateCardResponse>(request);
        }
    }
}
