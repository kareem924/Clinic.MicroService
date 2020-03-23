using System;
using System.Threading.Tasks;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;

namespace Portals.Extivita.Core.Appointments.Services
{
    public interface ITherapySessionService
    {
        Task<TherapySession> GetOrCreateIfNotExistsAsync(ISessionDetails details);

        Task<IHaveSessionDetails> GetSessionDetails(ISessionDetails details);
    }
}