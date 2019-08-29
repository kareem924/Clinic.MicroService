using Appointment.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appointment.Infrastructure.Interfaces
{
    public interface IAppointmentDbContext
    {
         IMongoCollection<Session> Session { get; }

         IMongoCollection<Core.Entities.Appointment> Appointment { get; }
    }
}
