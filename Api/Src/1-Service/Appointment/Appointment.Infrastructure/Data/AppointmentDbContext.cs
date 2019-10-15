using Appointment.Core.Entities;
using Appointment.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace Appointment.Infrastructure.Data
{
    public class AppointmentDbContext : IAppointmentDbContext
    {
        private readonly IMongoDatabase database;


        public AppointmentDbContext()
        {
          
        }

        public IMongoCollection<Session> Session => database.GetCollection<Session>("Session");

        public IMongoCollection<Core.Entities.Appointment> Appointment => 
            database.GetCollection<Core.Entities.Appointment>("Appointment");
    }
}

