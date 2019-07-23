using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Clinic.SharedKernel.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data.UnitOfWork
{
    public abstract class Repository<TObject> : IRepository<TObject> where TObject : class
    {
        protected IUnitOfWork UnitOfWork;

        /// <summary>
        /// The contructor requires an open DataContext to work with
        /// </summary>
        /// <param name="context">An open DataContext</param>
        protected Repository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
        /// <summary>
        /// The contructor requires an open DataContext to work with
        /// </summary>
        /// <param name="unitOfWork"></param>


        /// <summary>
        /// Returns a single object with a primary key of the provided id
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="id">The primary key of the object to fetch</param>
        /// <returns>A single object with the provided primary key or null</returns>
        /// 
        //
        public TObject Get(object id)
        {

            return UnitOfWork.Set<TObject>().Find(id);
        }
        /// <summary>
        /// Returns a single object with a primary key of the provided id
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="id">The primary key of the object to fetch</param>
        /// <returns>A single object with the provided primary key or null</returns>
        public async Task<TObject> GetAsync(int id)
        {
            return await UnitOfWork.Set<TObject>().FindAsync(id);
        }
        /// <summary>
        /// Gets a collection of all objects in the database
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <returns>An ICollection of every object in the database</returns>
        public ICollection<TObject> GetAll()
        {
            return UnitOfWork.Set<TObject>().ToList();
        }

        public ICollection<TObject> GetAll<TKey>(int take, int skip, Expression<Func<TObject, TKey>> orderBy)
        {
            //ICollection<TObject> t = Entities.OrderBy(OrderBy).Skip(Skip).Take(Take).ToList();
            return UnitOfWork.Set<TObject>().OrderByDescending(orderBy).Skip(skip).Take(take).ToList();
        }
        /// <summary>
        /// Gets a collection of all objects in the database
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>An ICollection of every object in the database</returns>
        public async Task<ICollection<TObject>> GetAllAsync()
        {
            return await UnitOfWork.Set<TObject>().ToListAsync();
        }

        /// <summary>
        /// Returns a single object which matches the provided expression
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A Linq expression filter to find a single result</param>
        /// <param name="includes"></param>
        /// <returns>A single object which matches the expression filter. 
        /// If more than one object is found or if zero are found, null is returned</returns>
        public TObject Find(Expression<Func<TObject, bool>> match, string[] includes = null)
        {
            IQueryable<TObject> query = UnitOfWork.Set<TObject>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.SingleOrDefault(match);
        }

        /// <summary>
        /// Returns a single object which matches the provided expression
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="match">A Linq expression filter to find a single result</param>
        /// <param name="includes"></param>
        /// <returns>A single object which matches the expression filter. 
        /// If more than one object is found or if zero are found, null is returned</returns>
        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match, string[] includes = null)
        {
            IQueryable<TObject> query = UnitOfWork.Set<TObject>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.SingleOrDefaultAsync(match);
        }
        /// <summary>
        /// Returns a collection of objects which match the provided expression
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>An ICollection of object which match the expression filter</returns>
        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match, string[] includes = null)
        {
            IQueryable<TObject> query = UnitOfWork.Set<TObject>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query.Where(match).ToList();
        }


        public ICollection<TObject> FindAll<TKey>(Expression<Func<TObject, bool>> match, int take, int skip, Expression<Func<TObject, TKey>> orderBy, string[] includes = null)
        {
            IQueryable<TObject> query = UnitOfWork.Set<TObject>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            //var query1 = query.Where(match);
            return query.Where(match).OrderByDescending(orderBy).Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Returns a collection of objects which match the provided expression
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <param name="match">A linq expression filter to find one or more results</param>
        /// <returns>An ICollection of object which match the expression filter</returns>
        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match, string[] includes = null)
        {
            IQueryable<TObject> query = UnitOfWork.Set<TObject>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.Where(match).ToListAsync();
        }
        /// <summary>
        /// Inserts a single object to the database and commits the change
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="t">The object to insert</param>
        /// <returns>The resulting object including its primary key after the insert</returns>
        public TObject Add(TObject t)
        {
            UnitOfWork.Set<TObject>().Add(t);
            return t;
        }

        /// <summary>
        /// Inserts a single object to the database and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>The resulting object including its primary key after the insert</returns>
        /// <summary>
        /// Inserts a collection of objects into the database and commits the changes
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="tList">An IEnumerable list of objects to insert</param>
        /// <returns>The IEnumerable resulting list of inserted objects including the primary keys</returns>
        public IEnumerable<TObject> AddAll(IEnumerable<TObject> tList)
        {
            var addAll = tList.ToList();
            UnitOfWork.Set<TObject>().AddRange(addAll);
            return addAll;
        }
        /// <summary>
        /// Updates a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="updated">The updated object to apply to the database</param>
        /// <param name="key">The primary key of the object to update</param>
        /// <returns>The resulting updated object</returns>
        public TObject Update(TObject updated, int key)
        {
            if (updated == null)
                return null;

            TObject existing = UnitOfWork.Set<TObject>().Find(key);
            if (existing != null)
            {
                UnitOfWork.Entry(existing).CurrentValues.SetValues(updated);
            }
            return existing;
        }
        /// <summary>
        /// Updates a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="updated">The updated object to apply to the database</param>
        /// <param name="key">The primary key of the object to update when Key is Long Not Int</param>
        /// <returns>The resulting updated object</returns>
        public TObject Update(TObject updated, long key)
        {
            if (updated == null)
                return null;

            TObject existing = UnitOfWork.Set<TObject>().Find(key);
            if (existing != null)
            {
                UnitOfWork.Entry(existing).CurrentValues.SetValues(updated);
            }
            return existing;
        }
        public void Attach(TObject t)
        {
            UnitOfWork.Set<TObject>().Attach(t);
        }

        /// <summary>
        /// Updates a single object based on the provided primary key and commits the change
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>The resulting updated object</returns>
        /// <summary>
        /// Deletes a single object from the database and commits the change
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <param name="t">The object to delete</param>
        public void Delete(TObject t)
        {
            UnitOfWork.Set<TObject>().Remove(t);
        }

        /// <summary>
        /// Gets the count of the number of objects in the databse
        /// </summary>
        /// <remarks>Synchronous</remarks>
        /// <returns>count number of all objects</returns>
        public int Count()
        {
            return UnitOfWork.Set<TObject>().Count();
        }


        /// <summary>
        /// get objects count which match the provided expression 
        /// </summary>
        /// <param name="match">A linq expression filter</param>
        /// <returns>objects count</returns>
        public int Count(Expression<Func<TObject, bool>> match)
        {
            return UnitOfWork.Set<TObject>().Where(match).Count();
        }
        /// <summary>
        /// Gets the count of the number of objects in the databse
        /// </summary>
        /// <remarks>Asynchronous</remarks>
        /// <returns>The count of the number of objects</returns>
        public async Task<int> CountAsync()
        {
            return await UnitOfWork.Set<TObject>().CountAsync();
        }

        public async Task<TObject> GetAsync(object id)
        {
            var item = await UnitOfWork.Set<TObject>().FindAsync(id);
            return item;
        }

        public async Task<ICollection<TObject>> FindAllAsync<TKey>(Expression<Func<TObject, bool>> match, int take, int skip, Expression<Func<TObject, TKey>> orderBy, string[] includes = null)
        {
            IQueryable<TObject> query = UnitOfWork.Set<TObject>().Where(match).OrderByDescending(orderBy).Skip(skip).Take(take);
            if (includes == null) return await query.ToListAsync();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
        public async Task<TObject> AddAsync(TObject t)
        {
            await UnitOfWork.Set<TObject>().AddAsync(t);
            return t;
        }
        public async Task<int> CountAsync(Expression<Func<TObject, bool>> match)
        {
            return await UnitOfWork.Set<TObject>().Where(match).CountAsync();
        }

        public async Task<IEnumerable<TObject>> AddAllAsync(IEnumerable<TObject> tList)
        {

            var addAll = tList.ToList();
            await UnitOfWork.Set<TObject>().AddRangeAsync(addAll);
            return addAll;
        }

        public TObject Update(TObject updated, object key)
        {
            if (updated == null)
                return null;

            TObject existing = UnitOfWork.Set<TObject>().Find(key);
            if (existing != null)
            {
                UnitOfWork.Entry(existing).CurrentValues.SetValues(updated);
            }
            return existing;
        }

        public async Task<TObject> UpdateAsync(TObject updated, object key)
        {
            if (updated == null)
                return null;

            TObject existing = await UnitOfWork.Set<TObject>().FindAsync(key);
            if (existing != null)
            {
                UnitOfWork.Entry(existing).CurrentValues.SetValues(updated);
            }
            return existing;
        }
    }

}
