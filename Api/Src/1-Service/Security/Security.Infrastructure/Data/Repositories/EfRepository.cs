﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.General.Entity;
using Common.General.Interfaces;
using Common.General.Repository;
using Common.General.Specification;
using Common.General.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Security.Infrastructure.Data.Repositories
{
    public abstract class EfRepository<T> : ISpecificationRepository<T> where T : class, IEntity, IAggregateRoot
    {
        private readonly IUnitOfWork _unitOfWork;

        protected EfRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public T GetById(object id)
        {
            return _unitOfWork.Context.Set<T>().Find(id);

        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _unitOfWork.Context.Set<T>().FindAsync(id);
        }

        public T Find(ISpecification<T> spec)
        {
            return ApplySpecification(spec).SingleOrDefault();
        }

        public async Task<T> FindAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).SingleOrDefaultAsync();
        }

        public ICollection<T> FindAll(ISpecification<T> specification)
        {
            return ApplySpecification(specification).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
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

        public Task<T> UpdateAsync(T updated, object key)
        {

            _unitOfWork.Context.Entry(updated).State = EntityState.Modified;

            return Task.FromResult(updated);
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
            return ApplySpecification(spec).Count();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_unitOfWork.Context.Set<T>().AsQueryable(), spec);
        }

        public async Task<PagedResult<T>> GetAllPagedAsync(ISpecification<T> specification, PagedQueryBase query)
        {
            var totalResults = await _unitOfWork.Context.Set<T>().CountAsync();
            var totalPages = (int)Math.Ceiling((decimal)totalResults / query.PageSize);
            var data =  ApplySpecification(specification).ToList(); ;
            return PagedResult<T>.Create(data, query.Page, query.PageSize, totalPages, totalResults);            
        }
    }
}
