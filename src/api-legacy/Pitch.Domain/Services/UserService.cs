using Pitch.DataStorage.Contexts;
using Pitch.DataStorage.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Pitch.Domain.Services
{
    public interface IUserService
    {
        Task<User> GetOrCreate(string name, string email);
    }

    public class UserService : IUserService
    {
        private readonly PitchContext _pitchContext;

        public UserService(PitchContext pitchContext)
        {
            _pitchContext = pitchContext;
        }

        public async Task<User> GetOrCreate(string name, string email)
        {
            var user = _pitchContext.Users.SingleOrDefault(x => x.Email == email);
            if (user == null)
            {
                user = new User() { Name = name, Email = email };
                _pitchContext.Users.Add(user);
                await _pitchContext.SaveChangesAsync();
            }
            return user;
        }
    }
}
