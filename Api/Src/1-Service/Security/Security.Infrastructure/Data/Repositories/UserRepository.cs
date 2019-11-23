using Common.General.UnitOfWork;
using Security.Core.Entities;
using Security.Core.Repositories;

namespace Security.Infrastructure.Data.Repositories
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
