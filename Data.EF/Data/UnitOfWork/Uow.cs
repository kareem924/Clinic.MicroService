using System.Threading.Tasks;
using Clinic.SharedKernel.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Auth.Infrastructure.Data.UnitOfWork
{
    
    public class Uow : IUnitOfWork
    {
        private readonly DbContext _context;
        public EntityEntry Entry(object entity)
        {
            return _context.Entry(entity);
        }

        public DbSet<T> Set<T>() where T : class
        {
            return _context.Set<T>();
        }

        private bool _isDisposed;

        public Uow()
        {
            _context = new ClinicDbContext();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            _context.Dispose();
        }
    }

}
