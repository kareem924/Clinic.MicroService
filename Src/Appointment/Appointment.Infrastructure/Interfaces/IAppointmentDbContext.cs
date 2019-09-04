using Appointment.Core.Entities;
using MongoDB.Driver;

namespace Appointment.Infrastructure.Interfaces
{
    public interface IAppointmentDbContext
    {
         IMongoCollection<Session> Session { get; }

         IMongoCollection<Core.Entities.Appointment> Appointment { get; }
    }
}
