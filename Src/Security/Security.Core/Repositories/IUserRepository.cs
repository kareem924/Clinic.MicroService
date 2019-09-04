using System.Threading.Tasks;
using Common.General.Repository;
using Security.Core.Entities;

namespace Security.Core.Repositories
{
    public interface IUserRepository : ISpecificationRepository<User>
    {
        Task<User> FindByName(string userName);
        Task<bool> CheckPassword(User user, string password);
    }
}
