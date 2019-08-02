using System.Collections.Generic;
using System.Threading.Tasks;
using Common.General.Entity;
using Common.General.Specification;

namespace Common.General.Repository
{
    public interface ISpecificationRepository<T> where T : IEntity<T>

    {
        T Get(object id);

        Task<T> GetAsync(object id);

        T Find(ISpecification<T> spec);
     
        Task<T> FindAsync(ISpecification<T> spec);
  
        ICollection<T> FindAll(ISpecification<T> spec);
     
        Task<ICollection<T>> FindAllAsync(ISpecification<T> spec);
        
        T Add(T t);
     
        Task<T> AddAsync(T t);
       
        IEnumerable<T> AddAll(IEnumerable<T> tList);
     
        Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> tList);
      
        T Update(T updated, object key);
        
        Task<T> UpdateAsync(T updated, object key);
       
        void Delete(T t);
      
        void Attach(T t);

        int Count(ISpecification<T> spec);
       
        Task<int> CountAsync(ISpecification<T> spec);

    }
}
