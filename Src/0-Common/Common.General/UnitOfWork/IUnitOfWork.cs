using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Common.General.UnitOfWork
{
    public interface IUnitOfWork
    {
        DbContext Context { get; }

        void Commit();

        Task<int> CommitAsync();
    }
}
