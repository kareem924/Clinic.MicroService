using Common.General.UnitOfWork;
using Security.Core.Entities;
using Security.Core.Repositories;

namespace Security.Infrastructure.Data.Repositories
{
    public class RoleRepository : EfRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
