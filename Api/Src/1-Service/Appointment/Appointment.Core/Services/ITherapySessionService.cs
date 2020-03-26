using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Appointment.Core.Entities;

namespace Appointment.Core.Services
{
    public interface ISessionService
    {
        Task<Session> GetOrCreateIfNotExistsAsync(ISessionDetails details);

        Task<IHaveSessionDetails> GetSessionDetails(ISessionDetails details);
    }
}
