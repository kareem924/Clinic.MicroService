using Appointment.Core.Entities;
using Common.General.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Appointment.Infrastructure.Data.Repositories
{
    public class SessionRepository : IRepository<Session>
    {
        private readonly IMongoDatabase _db;

        public SessionRepository(IMongoDatabase dbContext)
        {
            _db = dbContext;
        }

        public Session Add(Session entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }


        public async Task<Session> AddAsync(Session entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public IEnumerable<Session> AddRange(IReadOnlyCollection<Session> entityList)
        {
            Collection.InsertMany(entityList);
            return entityList;
        }


        public async Task<IEnumerable<Session>> AddRangeAsync(IReadOnlyCollection<Session> entityList)
        {
            await Collection.InsertManyAsync(entityList);
            return entityList;
        }



        public void Attach(Session entity)
        {
            throw new NotImplementedException();
        }


        public long Count(Expression<Func<Session, bool>> match)
        {
            return Collection.CountDocuments(match);
        }

        public async Task<long> CountAsync(Expression<Func<Session, bool>> match)
        {
            return await Collection.CountDocumentsAsync(match);
        }

        public void Delete(Session entity)
        {
            Collection.DeleteOne(session => session.Id == entity.Id);
        }


        public async Task<IEnumerable<Session>> GetAllAsync()
        {
            return await(await Collection.FindAsync(_ => true)).ToListAsync();
        }

        public Session Find(Expression<Func<Session, bool>> match, string[] includes = null)
        {
            return Collection.Find(match).FirstOrDefault();
        }

        public ICollection<Session> FindAll(Expression<Func<Session, bool>> match, string[] includes = null)
        {
            return Collection.Find(match).ToList();
        }

        public ICollection<Session> FindAll<TKey>(Expression<Func<Session, bool>> match, int take, int skip, Expression<Func<Session, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Session>> FindAllAsync(Expression<Func<Session, bool>> match, string[] includes = null)
        {
            return await (await Collection.FindAsync(match)).ToListAsync();
        }

        public Task<ICollection<Session>> FindAllAsync<TKey>(Expression<Func<Session, bool>> match, int take, int skip, Expression<Func<Session, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }



        public async Task<Session> FindAsync(Expression<Func<Session, bool>> match, string[] includes = null)
        {
            return await (await Collection.FindAsync(match)).FirstOrDefaultAsync();
        }

        public Session Get(object id)
        {
            return Collection.Find(session => session.Id == (Guid)id).FirstOrDefault();
        }

        public async Task<Session> GetAsync(object id)
        {
            return await (await Collection.FindAsync(session => session.Id == (Guid)id))
                .FirstOrDefaultAsync();
        }

        public IEnumerable<Session> GetAll()
        {
           return Collection.Find(_ => true).ToList();
        }

        public Session Update(Session updated, object id)
        {
            Collection.ReplaceOne(session => session.Id == (Guid)id, updated);
            return updated;
        }

        public async Task<Session> UpdateAsync(Session updated, object id)
        {
           await Collection.ReplaceOneAsync(session => session.Id == (Guid)id, updated);
            return updated;
        }

        private IMongoCollection<Session> Collection
          => _db.GetCollection<Session>("Session");
    }
}
