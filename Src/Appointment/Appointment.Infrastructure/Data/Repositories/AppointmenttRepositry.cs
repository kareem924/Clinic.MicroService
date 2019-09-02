using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Appointment.Infrastructure.Interfaces;
using Common.General.Repository;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Appointment.Infrastructure.Data.Repositories
{
    public class AppointmentRepository : IRepository<Core.Entities.Appointment>
    {
        private readonly IAppointmentDbContext _dbContext;

        public AppointmentRepository(IAppointmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Core.Entities.Appointment Add(Core.Entities.Appointment entity)
        {
            _dbContext.Appointment.InsertOne(entity);
            return entity;
        }


        public async Task<Core.Entities.Appointment> AddAsync(Core.Entities.Appointment entity)
        {
            await _dbContext.Appointment.InsertOneAsync(entity);
            return entity;
        }

        public IEnumerable<Core.Entities.Appointment> AddRange(IReadOnlyCollection<Core.Entities.Appointment> entityList)
        {
            _dbContext.Appointment.InsertMany(entityList);
            return entityList;
        }


        public async Task<IEnumerable<Core.Entities.Appointment>> AddRangeAsync(IReadOnlyCollection<Core.Entities.Appointment> entityList)
        {
            await _dbContext.Appointment.InsertManyAsync(entityList);
            return entityList;
        }



        public void Attach(Core.Entities.Appointment entity)
        {
            throw new NotImplementedException();
        }


        public long Count(Expression<Func<Core.Entities.Appointment, bool>> match)
        {
            return _dbContext.Appointment.CountDocuments(match);
        }

        public async Task<long> CountAsync(Expression<Func<Core.Entities.Appointment, bool>> match)
        {
            return await _dbContext.Appointment.CountDocumentsAsync(match);
        }

        public void Delete(Core.Entities.Appointment entity)
        {
            _dbContext.Appointment.DeleteOne(appointment => appointment.Id == entity.Id);
        }


        public async Task<IEnumerable<Core.Entities.Appointment>> GetAllAsync()
        {
            return await _dbContext.Appointment.AsQueryable().ToArrayAsync();
        }

        public Core.Entities.Appointment Find(Expression<Func<Core.Entities.Appointment, bool>> match, string[] includes = null)
        {
            return _dbContext.Appointment.Find(match).FirstOrDefault();
        }

        public ICollection<Core.Entities.Appointment> FindAll(Expression<Func<Core.Entities.Appointment, bool>> match, string[] includes = null)
        {
            return _dbContext.Appointment.Find(match).ToList();
        }

        public ICollection<Core.Entities.Appointment> FindAll<TKey>(Expression<Func<Core.Entities.Appointment, bool>> match, int take, int skip, Expression<Func<Core.Entities.Appointment, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Core.Entities.Appointment>> FindAllAsync(Expression<Func<Core.Entities.Appointment, bool>> match, string[] includes = null)
        {
            return await (await _dbContext.Appointment.FindAsync(match)).ToListAsync();
        }

        public Task<ICollection<Core.Entities.Appointment>> FindAllAsync<TKey>(Expression<Func<Core.Entities.Appointment, bool>> match, int take, int skip, Expression<Func<Core.Entities.Appointment, TKey>> orderBy, string[] includes = null)
        {
            throw new NotImplementedException();
        }



        public async Task<Core.Entities.Appointment> FindAsync(Expression<Func<Core.Entities.Appointment, bool>> match, string[] includes = null)
        {
            return await (await _dbContext.Appointment.FindAsync(match)).FirstOrDefaultAsync();
        }

        public Core.Entities.Appointment Get(object id)
        {
            return _dbContext.Appointment.Find(appointment => appointment.Id == (ObjectId)id).FirstOrDefault();
        }

        public async Task<Core.Entities.Appointment> GetAsync(object id)
        {
            return await (await _dbContext.Appointment.FindAsync(appointment => appointment.Id == (ObjectId)id))
                .FirstOrDefaultAsync();
        }

        public IEnumerable<Core.Entities.Appointment> GetAll()
        {
            return _dbContext.Appointment.AsQueryable().ToArray();
        }

        public Core.Entities.Appointment Update(Core.Entities.Appointment updated, object id)
        {
            _dbContext.Appointment.ReplaceOne(appointment => appointment.Id == (ObjectId)id, updated);
            return updated;
        }

        public async Task<Core.Entities.Appointment> UpdateAsync(Core.Entities.Appointment updated, object id)
        {
            await _dbContext.Appointment.ReplaceOneAsync(appointment => appointment.Id == (ObjectId)id, updated);
            return updated;
        }
    }
}
