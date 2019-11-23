using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.General.Entity;
using Common.General.Interfaces;
using Common.General.Repository;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Common.MongoDb
{
    public abstract class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity<Guid>,IAggregateRoot
    {
        protected IMongoCollection<TEntity> Collection { get; }

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            Collection = database.GetCollection<TEntity>(collectionName);
        }

        public TEntity Add(TEntity t)
        {
            Collection?.InsertOne(t);
            return t;
        }

        public async Task<TEntity> AddAsync(TEntity t)
        {
            await Collection.InsertOneAsync(t);
            return t;
        }

        public IEnumerable<TEntity> AddRange(IReadOnlyCollection<TEntity> tList)
        {
            Collection.InsertMany(tList);
            return tList;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IReadOnlyCollection<TEntity> tList)
        {
            await Collection.InsertManyAsync(tList);
            return tList;
        }

        public void Attach(TEntity t)
        {
            throw new NotImplementedException("Not supported For MongoDB");
        }

        public long Count(Expression<Func<TEntity, bool>> match) => Collection.CountDocuments(match);

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> match) => await Collection.CountDocumentsAsync(match);

        public void Delete(TEntity t)
        {
            Collection.DeleteOne(entity => entity.Id == t.Id);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> match, string[] includes = null) =>
            Collection.Find(match).FirstOrDefault();

        public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> match, string[] includes = null)
        {
            return Collection.Find(match).ToList();
        }

        public ICollection<TEntity> FindAll<TKey>(
            Expression<Func<TEntity, bool>> match, 
            int take, 
            int skip, 
            Expression<Func<TEntity, TKey>> orderBy, 
            string[] includes = null)
        {
            throw new NotImplementedException("Not Supported In MongoDb");
        }

        public Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<TEntity>> FindAllAsync<TKey>(
            Expression<Func<TEntity, bool>> match,
            int take,
            int skip,
            Expression<Func<TEntity, TKey>> orderBy, 
            string[] includes = null) => await(await Collection.FindAsync(match)).ToListAsync();

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match, string[] includes = null)
        {
            throw new NotImplementedException("Not Supported in MongoDb");
        }

        public TEntity Get(object id) => Collection.Find(entity => entity.Id == (Guid)id).FirstOrDefault();

        public IEnumerable<TEntity> GetAll() => Collection.AsQueryable().ToArray();

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await Collection.AsQueryable().ToArrayAsync();

        public async Task<TEntity> GetAsync(object id)
        {
            return await(await Collection.FindAsync(entity => entity.Id == (Guid)id))
                .FirstOrDefaultAsync();
        }

        public TEntity Update(TEntity updated, object id)
        {
            Collection.ReplaceOne(appointment => appointment.Id == (Guid)id, updated);
            return updated;
        }

        public async Task<TEntity> UpdateAsync(TEntity updated, object id)
        {
            await Collection.ReplaceOneAsync(appointment => appointment.Id == (Guid)id, updated);
            return updated;
        }
    }
}
