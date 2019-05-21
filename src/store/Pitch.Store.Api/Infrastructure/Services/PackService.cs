using System;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Http;
using Pitch.Store.Api.Application.Requests;
using Pitch.Store.Api.Application.Responses;
using Pitch.Store.Api.Infrastructure.Repositories;
using Pitch.Store.Api.Models;

namespace Pitch.Store.Api.Infrastructure.Services
{
    public interface IPackService
    {
        Task<CreateCardResponse> Open(Guid id);
        Task<Guid> Buy();
    }
    public class PackService : IPackService
    {
        private readonly IPackRepository _packRepository;
        private readonly IBus _bus;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PackService(IPackRepository packRepository, IBus bus, IHttpContextAccessor httpContextAccessor)
        {
            _packRepository = packRepository;
            _bus = bus;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateCardResponse> Open(Guid id)
        {
            var pack = await _packRepository.GetAsync(id);
            //check logged in userid matches card userid

            var userId = Guid.NewGuid(); //todo get userid from auth
            var request = new CreateCardRequest(userId);
            return await _bus.RequestAsync<CreateCardRequest, CreateCardResponse>(request);
        }

        public async Task<Guid> Buy()
        {
            var pack = new Pack();
            //pack.UserId = _httpContextAccessor.HttpContext.User.
            var @new = await _packRepository.AddAsync(pack);
            await _packRepository.SaveChangesAsync();
            return @new.Entity.Id; 
        }
    }
}
