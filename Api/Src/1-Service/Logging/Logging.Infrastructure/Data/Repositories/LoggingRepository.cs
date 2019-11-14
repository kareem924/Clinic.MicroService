using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.General.Repository;
using Logging.Core.Entities;
using MongoDB.Driver;

namespace Logging.Infrastructure.Data.Repositories
{
   public class LoggingRepository : IRepository<LogEntry>
    {
        private readonly IMongoDatabase _db;

        public LoggingRepository(IMongoDatabase db)
        {
            _db = db;
        }

        public LogEntry Get(object id)
        {
            throw new NotImplementedException();
        }

        public Task<LogEntry> GetAsync(object id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LogEntry> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LogEntry>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public LogEntry Find(Expression<Func<LogEntry, bool>> match, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<LogEntry> FindAsync(Expression<Func<LogEntry, bool>> match, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public ICollection<LogEntry> FindAll(Expression<Func<LogEntry, bool>> match, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<LogEntry>> FindAllAsync(Expression<Func<LogEntry, bool>> match, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public ICollection<LogEntry> FindAll<TKey>(Expression<Func<LogEntry, bool>> match, int take, int skip, Expression<Func<LogEntry, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<LogEntry>> FindAllAsync<TKey>(Expression<Func<LogEntry, bool>> match, int take, int skip, Expression<Func<LogEntry, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public LogEntry Add(LogEntry t)
        {
            Collection.InsertOne(t);
            return t;
        }

        public async Task<LogEntry> AddAsync(LogEntry t)
        {
            await Collection.InsertOneAsync(t);
            return t;
        }

        public IEnumerable<LogEntry> AddRange(IReadOnlyCollection<LogEntry> tList)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LogEntry>> AddRangeAsync(IReadOnlyCollection<LogEntry> tList)
        {
            throw new NotImplementedException();
        }

        public LogEntry Update(LogEntry updated, object id)
        {
            throw new NotImplementedException();
        }

        public Task<LogEntry> UpdateAsync(LogEntry updated, object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(LogEntry t)
        {
            throw new NotImplementedException();
        }

        public void Attach(LogEntry t)
        {
            throw new NotImplementedException();
        }

        public long Count(Expression<Func<LogEntry, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(Expression<Func<LogEntry, bool>> match)
        {
            throw new NotImplementedException();
        }
        private IMongoCollection<LogEntry> Collection
            => _db.GetCollection<LogEntry>("LogEntries");
    }
}
