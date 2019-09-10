using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.General.Repository;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Appointment.Infrastructure.Data.Repositories
{
    public class AppointmentRepository : IRepository<Core.Entities.Appointment>
    {
        private readonly IMongoDatabase _db;

        public AppointmentRepository(IMongoDatabase dbContext)
        {
            _db = dbContext;
        }

        public Core.Entities.Appointment Add(Core.Entities.Appointment entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }


        public async Task<Core.Entities.Appointment> AddAsync(Core.Entities.Appointment entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public IEnumerable<Core.Entities.Appointment> AddRange(IReadOnlyCollection<Core.Entities.Appointment> entityList)
        {
            Collection.InsertMany(entityList);
            return entityList;
        }


        public async Task<IEnumerable<Core.Entities.Appointment>> AddRangeAsync(IReadOnlyCollection<Core.Entities.Appointment> entityList)
        {
            await Collection.InsertManyAsync(entityList);
            return entityList;
        }



        public void Attach(Core.Entities.Appointment entity)
        {
            throw new NotImplementedException();
        }


        public long Count(Expression<Func<Core.Entities.Appointment, bool>> match)
        {
            return Collection.CountDocuments(match);
        }

        public async Task<long> CountAsync(Expression<Func<Core.Entities.Appointment, bool>> match)
        {
            return await Collection.CountDocumentsAsync(match);
        }

        public void Delete(Core.Entities.Appointment entity)
        {
            Collection.DeleteOne(appointment => appointment.Id == entity.Id);
        }


        public async Task<IEnumerable<Core.Entities.Appointment>> GetAllAsync()
        {
            return await Collection.AsQueryable().ToArrayAsync();
        }

        public Core.Entities.Appointment Find(Expression<Func<Core.Entities.Appointment, bool>> match, string[] includes = null)
        {
            return Collection.Find(match).FirstOrDefault();
        }

        public ICollection<Core.Entities.Appointment> FindAll(Expression<Func<Core.Entities.Appointment, bool>> match, string[] includes = null)
        {
            return Collection.Find(match).ToList();
        }

        public ICollection<Core.Entities.Appointment> FindAll<TKey>(Expression<Func<Core.Entities.Appointment, bool>> match, int take, int skip, Expression<Func<Core.Entities.Appointment, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Core.Entities.Appointment>> FindAllAsync(Expression<Func<Core.Entities.Appointment, bool>> match, string[] includes = null)
        {
            return await (await Collection.FindAsync(match)).ToListAsync();
        }

        public Task<ICollection<Core.Entities.Appointment>> FindAllAsync<TKey>(Expression<Func<Core.Entities.Appointment, bool>> match, int take, int skip, Expression<Func<Core.Entities.Appointment, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }



        public async Task<Core.Entities.Appointment> FindAsync(Expression<Func<Core.Entities.Appointment, bool>> match, string[] includes = null)
        {
            return await (await Collection.FindAsync(match)).FirstOrDefaultAsync();
        }

        public Core.Entities.Appointment Get(object id)
        {
            return Collection.Find(appointment => appointment.Id == (Guid)id).FirstOrDefault();
        }

        public async Task<Core.Entities.Appointment> GetAsync(object id)
        {
            return await (await Collection.FindAsync(appointment => appointment.Id == (Guid)id))
                .FirstOrDefaultAsync();
        }

        public IEnumerable<Core.Entities.Appointment> GetAll()
        {
            return Collection.AsQueryable().ToArray();
        }

        public Core.Entities.Appointment Update(Core.Entities.Appointment updated, object id)
        {
            Collection.ReplaceOne(appointment => appointment.Id == (Guid)id, updated);
            return updated;
        }

        public async Task<Core.Entities.Appointment> UpdateAsync(Core.Entities.Appointment updated, object id)
        {
            await Collection.ReplaceOneAsync(appointment => appointment.Id == (Guid)id, updated);
            return updated;
        }
        private IMongoCollection<Core.Entities.Appointment> Collection
           => _db.GetCollection<Core.Entities.Appointment>("Appointment");
    }
}
