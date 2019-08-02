using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.General.Entity;

namespace Common.General.Repository
{
    public interface IRepository<T> where T : IEntity<T>

    {        
        T Get(object id);
     
        Task<T> GetAsync(object id);

        T Find(Expression<Func<T, bool>> match, string[] includes = null);
    
        Task<T> FindAsync(Expression<Func<T, bool>> match, string[] includes = null);

        ICollection<T> FindAll(Expression<Func<T, bool>> match, string[] includes = null);
       
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match, string[] includes = null);

        ICollection<T> FindAll<TKey>(Expression<Func<T, bool>> match, int take, int skip, Expression<Func<T, TKey>> orderBy, string[] includes = null);

        Task<ICollection<T>> FindAllAsync<TKey>(Expression<Func<T, bool>> match, int take, int skip, Expression<Func<T, TKey>> orderBy, string[] includes = null);

        T Add(T t);
        
        Task<T> AddAsync(T t);

        IEnumerable<T> AddAll(IEnumerable<T> tList);
     
        Task<IEnumerable<T>> AddAllAsync(IEnumerable<T> tList);

        T Update(T updated, object key);

        Task<T> UpdateAsync(T updated, object key);

       void Delete(T t);
       
        void Attach(T t);

        int Count(Expression<Func<T, bool>> match);

        Task<int> CountAsync(Expression<Func<T, bool>> match);

    }
}