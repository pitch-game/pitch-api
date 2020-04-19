using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;
using Pitch.Store.API.Application.Requests;
using Pitch.Store.API.Application.Responses;
using Pitch.Store.API.Infrastructure.Repositories;
using Pitch.Store.API.Models;

namespace Pitch.Store.API.Infrastructure.Services
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

        public PackService(IPackRepository packRepository, IBus bus)
        {
            _packRepository = packRepository;
            _bus = bus;
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
            return response;
        }

        public async Task<Guid> Buy(Guid userId)
        {
            var pack = new Pack() { Id = Guid.NewGuid(), UserId = userId.ToString() };
            await _packRepository.AddAsync(pack);
            return pack.Id; 
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
        }

        private async Task<Pack> AddPack(Guid userId)
        {
            var pack = new Pack() { Id = Guid.NewGuid(), UserId = userId.ToString() };
            await _packRepository.AddAsync(pack);
            return pack;
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
        }
    }
}
