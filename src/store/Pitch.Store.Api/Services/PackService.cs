using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pitch.Store.Api.Application.Requests;
using Pitch.Store.Api.Application.Responses;
using Pitch.Store.Api.Infrastructure.Repositories;
using Pitch.Store.Api.Models;

namespace Pitch.Store.Api.Infrastructure.Services
{
    public interface IPackService
    {
        Task<IList<Pack>> GetAll(string userId);
        Task<CreateCardResponse> Open(Guid id, string userId);
        Task<Guid> Buy(Guid userId);
        Task CreateStartingPacksAsync(Guid userId);
        Task RedeemMatchRewards(Guid userId, bool victorious);
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

        public async Task<IList<Pack>> GetAll(string userId)
        {
            return await _packRepository.GetAllAsync(userId);
        }

        public async Task<CreateCardResponse> Open(Guid id, string userId)
        {
            var pack = await _packRepository.GetAsync(id);
            var request = new CreateCardRequest(userId);
            if (pack.Position != null)
            {
                request.Position = pack.Position;
            }
            var response = await _bus.RequestAsync<CreateCardRequest, CreateCardResponse>(request);
            await _packRepository.Delete(id);
            await _packRepository.SaveChangesAsync();
            return response;
        }

        public async Task<Guid> Buy(Guid userId)
        {
            var pack = new Pack() { Id = Guid.NewGuid(), UserId = userId.ToString() };
            var @new = await _packRepository.AddAsync(pack);
            await _packRepository.SaveChangesAsync();
            return @new.Entity.Id; 
        }

        public async Task RedeemMatchRewards(Guid userId, bool victorious)
        {
            if (victorious)
            {
                await AddPack(userId);
                await AddPack(userId);
            }
            //TODO Reward 1 pack for a draw and none for a loss
            await AddPack(userId);

            await _packRepository.SaveChangesAsync();
        }

        private async Task<EntityEntry<Pack>> AddPack(Guid userId)
        {
            var pack = new Pack() { Id = Guid.NewGuid(), UserId = userId.ToString() };
            return await _packRepository.AddAsync(pack);
        }

        public async Task CreateStartingPacksAsync(Guid userId)
        {
            var positions = new[] { "GK", "LB", "CB", "CB", "RB", "LM", "CM", "CM", "RM", "ST", "ST" };
            //TODO ask squad for positions
            foreach (var position in positions)
            {
                var pack = new Pack() { Id = Guid.NewGuid(), UserId = userId.ToString(), Position = position };
                await _packRepository.AddAsync(pack);
            }
            // 6 random
            for (int i = 0; i < 6; i++)
            {
                var pack = new Pack() { Id = Guid.NewGuid(), UserId = userId.ToString()};
                await _packRepository.AddAsync(pack);
            }
            await _packRepository.SaveChangesAsync();
        }
    }
}
