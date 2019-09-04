using System.Threading.Tasks;
using Common.General.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Security.Core.Entities;
using Security.Core.Repositories;

namespace Security.Infrastructure.Data.Repositories
{
    internal sealed class UserRepository : EfRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _uow;



        public UserRepository(UserManager<User> userManager, IUnitOfWork uow) : base(uow)
        {
            _userManager = userManager;
            _uow = uow;
        }

        public async Task<User> FindByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<bool> CheckPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
