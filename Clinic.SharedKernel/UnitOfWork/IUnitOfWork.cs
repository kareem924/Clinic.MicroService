using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Clinic.SharedKernel.UnitOfWork
{
    public interface IUnitOfWork
    {
        EntityEntry Entry(object entity);

        DbSet<T> Set<T>() where T : class;

        void Commit();

        Task<int> CommitAsync();
    }
}
