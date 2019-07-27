using System.Threading.Tasks;

namespace Common.Code.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();

        Task<int> CommitAsync();
    }
}
