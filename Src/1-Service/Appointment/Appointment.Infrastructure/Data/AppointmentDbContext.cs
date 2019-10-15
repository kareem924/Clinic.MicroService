using Appointment.Core.Entities;
using Appointment.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Appointment.Infrastructure.Data
{
    public class AppointmentDbContext : IAppointmentDbContext
    {
        private readonly MongoClient mongoClient;
        private readonly IMongoDatabase database;


        public AppointmentDbContext()
        {
          
        }

        public IMongoCollection<Session> Session
        {
            get
            {
                return database.GetCollection<Session>("Session");
            }
        }

        public IMongoCollection<Core.Entities.Appointment> Appointment
        {
            get
            {
                return database.GetCollection<Core.Entities.Appointment>("Appointment");
            }
        }
    }
}

