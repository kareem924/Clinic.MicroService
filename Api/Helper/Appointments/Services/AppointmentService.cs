using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.SharedKernel.Authentication;
using Portals.Extivita.SharedKernel.Communication.Email;

namespace Portals.Extivita.Core.Appointments.Services
{
    internal class AppointmentService : IAppointmentService
    {
        private readonly IUserSession _userSession;
        private readonly UserManager<User> _userManager;

        public AppointmentService(
            IUserSession userSession,
            UserManager<User> userManager)
        {
            _userSession = userSession;
            _userManager = userManager;
        }

        public async Task<Dictionary<string, string>> GetTemplateValues(Appointment appointment)
        {
            var patient = appointment.Patient;
            var date = GetStartDateTime(appointment);
            return new Dictionary<string, string>
            {
                {"user_name", patient.FullName},
                {"appointment_date_time", date.ToString(CultureInfo.CurrentCulture)},
                {"appointment_link", GetAppointmentLink()},
                {"email_unsubscribe_link", await GetUnsubscribeLink(patient.Id)}
            };
        }

        public CalendarEntry CreateCalendarEntry(Appointment appointment, CalendarEntryStatus status)
        {
            var start = GetStartDateTime(appointment);
            var end = GetEndDateTime(appointment);
            return new CalendarEntry(appointment.Id.ToString(), start, end, status, appointment.Version)
            {
                Title = "Extivita Clinic Appointment",
                Description = "Extivita Clinic Appointment Description"
            };
        }

        private DateTime GetStartDateTime(Appointment appointment)
        {
            var startDateTime = appointment.TherapySession.DateTime;
            return startDateTime;
        }

        private DateTime GetEndDateTime(Appointment appointment)
        {
            var endDateTime = appointment.TherapySession.DateTime + appointment.TherapySession.Duration;
            return endDateTime;
        }

        private string GetAppointmentLink()
        {
            //TODO: we should return a link to the specific appointment
            return _userSession.GetAbsoluteUrl("patient");
        }

        private async Task<string> GetUnsubscribeLink(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var token = await _userManager.GenerateUserTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider,
                "Unsubscribe From Email");
            var url = $"auth/users/{user.Id}/preferences/communication/email/unsubscribe/{token}";
            return _userSession.GetAbsoluteUrl(url);
        }
    }
}