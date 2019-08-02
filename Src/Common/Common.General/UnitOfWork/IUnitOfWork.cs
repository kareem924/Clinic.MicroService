using System.Threading.Tasks;

namespace Common.General.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();

        Task<int> CommitAsync();
    }
}
