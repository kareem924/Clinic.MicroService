using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.SharedKernel.Communication.Email;
namespace Portals.Extivita.Core.Appointments.Services
{
    public interface IAppointmentService
    {
        Task<Dictionary<string, string>> GetTemplateValues(Appointment appointment);

        CalendarEntry CreateCalendarEntry(Appointment appointment, CalendarEntryStatus status);
    }
}