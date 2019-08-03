using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.General.Entity;
using Common.General.Specification;

namespace Common.General.Repository
{
    public interface ISpecificationRepository<T> 

    {
        T Get(object id);

        Task<T> GetAsync(object id);

        T Find(ISpecification<T> spec);
     
        Task<T> FindAsync(ISpecification<T> spec);
  
        ICollection<T> FindAll(ISpecification<T> spec);
     
        Task<ICollection<T>> FindAllAsync(ISpecification<T> spec);
        
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
