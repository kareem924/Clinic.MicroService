using Appointment.Core.Entities;
using Appointment.Infrastructure.Interfaces;
using Common.General.Interfaces;
using Common.General.Repository;
using Common.General.Specification;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infrastructure.Data.Repositories
{
    public class SessionRepository : IRepository<Session>
    {
        private readonly IAppointmentDbContext _dbContext;

        public SessionRepository(IAppointmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Session Add(Session entity)
        {
            _dbContext.Session.InsertOne(entity);
            return entity;
        }


        public async Task<Session> AddAsync(Session entity)
        {
            await _dbContext.Session.InsertOneAsync(entity);
            return entity;
        }

        public IEnumerable<Session> AddRange(IReadOnlyCollection<Session> entityList)
        {
            _dbContext.Session.InsertMany(entityList);
            return entityList;
        }


        public async Task<IEnumerable<Session>> AddRangeAsync(IReadOnlyCollection<Session> entityList)
        {
            await _dbContext.Session.InsertManyAsync(entityList);
            return entityList;
        }



        public void Attach(Session entity)
        {
            throw new NotImplementedException();
        }


        public long Count(Expression<Func<Session, bool>> match)
        {
            return _dbContext.Session.CountDocuments(match);
        }

        public async Task<long> CountAsync(Expression<Func<Session, bool>> match)
        {
            return await _dbContext.Session.CountDocumentsAsync(match);
        }

        public void Delete(Session entity)
        {
            _dbContext.Session.DeleteOne(session => session.Id == entity.Id);
        }


        public async Task<IEnumerable<Session>> GetAllAsync()
        {
            return await(await _dbContext.Session.FindAsync(_ => true)).ToListAsync();
        }

        public Session Find(Expression<Func<Session, bool>> match, string[] includes = null)
        {
            return _dbContext.Session.Find(match).FirstOrDefault();
        }

        public ICollection<Session> FindAll(Expression<Func<Session, bool>> match, string[] includes = null)
        {
            return _dbContext.Session.Find(match).ToList();
        }

        public ICollection<Session> FindAll<TKey>(Expression<Func<Session, bool>> match, int take, int skip, Expression<Func<Session, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Session>> FindAllAsync(Expression<Func<Session, bool>> match, string[] includes = null)
        {
            return await (await _dbContext.Session.FindAsync(match)).ToListAsync();
        }

        public Task<ICollection<Session>> FindAllAsync<TKey>(Expression<Func<Session, bool>> match, int take, int skip, Expression<Func<Session, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }



        public async Task<Session> FindAsync(Expression<Func<Session, bool>> match, string[] includes = null)
        {
            return await (await _dbContext.Session.FindAsync(match)).FirstOrDefaultAsync();
        }

        public Session Get(object id)
        {
            return _dbContext.Session.Find(session => session.Id == (ObjectId)id).FirstOrDefault();
        }

        public async Task<Session> GetAsync(object id)
        {
            return await (await _dbContext.Session.FindAsync(session => session.Id == (ObjectId)id))
                .FirstOrDefaultAsync();
        }

        public IEnumerable<Session> GetAll()
        {
           return _dbContext.Session.Find(_ => true).ToList();
        }

        public Session Update(Session updated, object id)
        {
            _dbContext.Session.ReplaceOne(session => session.Id == (ObjectId)id, updated);
            return updated;
        }

        public async Task<Session> UpdateAsync(Session updated, object id)
        {
           await _dbContext.Session.ReplaceOneAsync(session => session.Id == (ObjectId)id, updated);
            return updated;
        }
    }
}
