using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.General.Entity;
using Common.General.Repository;
using Common.General.Specification;
using Common.General.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Security.Infrastructure.Data.Repositories
{
    public abstract class EfRepository<T> : ISpecificationRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public EfRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public T Get(object id)
        {
            return _unitOfWork.Context.Set<T>().Find(id);

        }


        public async Task<T> GetAsync(object id)
        {
            return await _unitOfWork.Context.Set<T>().FindAsync(id);
        }

        public T Find(ISpecification<T> spec)
        {
            throw new NotImplementedException();
        }

        public async Task<T> FindAsync(ISpecification<T> spec)
        {
            var result = await (List(spec));
            return result.FirstOrDefault();
        }

        public ICollection<T> FindAll(ISpecification<T> spec)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<T>> FindAllAsync(ISpecification<T> spec)
        {
            return await (List(spec));
        }

        public T Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _unitOfWork.Context.Set<T>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IReadOnlyCollection<T> entityList)
        {
            _unitOfWork.Context.Set<T>().AddRange(entityList);
            return entityList;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IReadOnlyCollection<T> entityList)
        {
            await _unitOfWork.Context.Set<T>().AddRangeAsync(entityList);
            return entityList;
        }

        public T Update(T updated, object key)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(key);
            if (existing != null)
            {
                _unitOfWork.Context.Entry(existing).CurrentValues.SetValues(updated);
            }
            return existing;
        }

        public async Task<T> UpdateAsync(T updated, object key)
        {
            T existing = await _unitOfWork.Context.Set<T>().FindAsync(key);
            if (existing != null)
            {
                _unitOfWork.Context.Entry(existing).CurrentValues.SetValues(updated);
            }
            return existing;
        }

        public void Delete(T entity)
        {
            _unitOfWork.Context.Set<T>().Remove(entity);
        }

        public void Attach(T entity)
        {
            _unitOfWork.Context.Set<T>().Attach(entity);
        }

        public int Count(ISpecification<T> spec)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var result = await (List(spec));
            return result.Count;
        }
        private async Task<List<T>> List(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_unitOfWork.Context.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await queryableResultWithIncludes
                .Where(spec.Criteria)
                .ToListAsync();
        }
    }
}
