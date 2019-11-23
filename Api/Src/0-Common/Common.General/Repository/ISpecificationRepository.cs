using System.Collections.Generic;
using System.Threading.Tasks;
using Common.General.Entity;
using Common.General.Interfaces;
using Common.General.Specification;

namespace Common.General.Repository
{
    public interface ISpecificationRepository<T> where T : IAggregateRoot

    {
        T GetById(object id);

        Task<T> GetByIdAsync(object id);

        T Find(ISpecification<T> spec);
     
        Task<T> FindAsync(ISpecification<T> specification);
  
        ICollection<T> FindAll(ISpecification<T> specification);
     
        Task<ICollection<T>> FindAllAsync(ISpecification<T> specification);

        Task<PagedResult<T>> GetAllPagedAsync(ISpecification<T> specification, PagedQueryBase query);

        T Add(T entity);
     
        Task<T> AddAsync(T entity);
       
        IEnumerable<T> AddRange(IReadOnlyCollection<T> entityList);
     
        Task<IEnumerable<T>> AddRangeAsync(IReadOnlyCollection<T> entityList);
      
        T Update(T updated, object key);
        
        Task<T> UpdateAsync(T updated, object key);
       
        void Delete(T entity);
      
        void Attach(T entity);

        int Count(ISpecification<T> spec);
       
        Task<int> CountAsync(ISpecification<T> spec);



    }
}
