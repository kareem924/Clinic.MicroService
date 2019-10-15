using System.Threading.Tasks;
using Common.General.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Security.Infrastructure.Data.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }

        private bool _isDisposed;

        public UnitOfWork(SecurityDbContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            Context.Dispose();
        }
    }
}
