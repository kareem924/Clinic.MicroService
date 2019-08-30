using Appointment.Core.Entities;
using Appointment.Infrastructure.Dto;
using Appointment.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Appointment.Infrastructure.Data
{
    public class AppointmentDbContext : IAppointmentDbContext
    {
        private readonly IMongoDatabase _database;


        public AppointmentDbContext(IOptions<MongoDbConfig> dbConfig)
        {
            var mongoClient = new MongoClient(dbConfig.Value.ConnectionString);
            _database = mongoClient.GetDatabase(dbConfig.Value.Database);
        }

        public IMongoCollection<Session> Session => _database.GetCollection<Session>("Session");

        public IMongoCollection<Core.Entities.Appointment> Appointment =>
            _database.GetCollection<Core.Entities.Appointment>("Appointment");
    }
}

