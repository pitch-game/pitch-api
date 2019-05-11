using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitch.Store.Api.Infrastructure.Repositories;

namespace Pitch.Store.Api.Infrastructure.Services
{
    public interface IPackService
    {
        Task<ActionResult<string>> Open(Guid id);
    }
    public class PackService : IPackService
    {
        private readonly IPackRepository _packRepository;

        public PackService(IPackRepository packRepository)
        {
            _packRepository = packRepository;
        }

        public async Task<ActionResult<string>> Open(Guid id)
        {
            var pack = await _packRepository.GetAsync(id);
            //check logged in userid matches card userid


            //todo Card command
            throw new NotImplementedException();
        }
    }
}
