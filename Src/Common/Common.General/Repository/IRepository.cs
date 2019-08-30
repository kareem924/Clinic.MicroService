using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.General.Entity;
using Common.General.Interfaces;

namespace Common.General.Repository
{

    public interface IRepository<T> where T : IAggregateRoot

    {        
        T Get(object id);
     
        Task<T> GetAsync(object id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T Find(Expression<Func<T, bool>> match, string[] includes = null);
    
        Task<T> FindAsync(Expression<Func<T, bool>> match, string[] includes = null);

        ICollection<T> FindAll(Expression<Func<T, bool>> match, string[] includes = null);
       
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match, string[] includes = null);

        ICollection<T> FindAll<TKey>(Expression<Func<T, bool>> match, int take, int skip, Expression<Func<T, TKey>> orderBy, string[] includes = null);

        Task<ICollection<T>> FindAllAsync<TKey>(Expression<Func<T, bool>> match, int take, int skip, Expression<Func<T, TKey>> orderBy, string[] includes = null);

        T Add(T t);
        
        Task<T> AddAsync(T t);

        IEnumerable<T> AddRange(IReadOnlyCollection<T> tList);
     
        Task<IEnumerable<T>> AddRangeAsync(IReadOnlyCollection<T> tList);

        T Update(T updated,object id);

        Task<T> UpdateAsync(T updated, object id);

       void Delete(T t);
       
        void Attach(T t);

        long Count(Expression<Func<T, bool>> match);

        Task<long> CountAsync(Expression<Func<T, bool>> match);

    }
}