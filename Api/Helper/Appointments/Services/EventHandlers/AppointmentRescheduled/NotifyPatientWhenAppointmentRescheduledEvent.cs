using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.Appointments.Events;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.Authentication.Preferences;
using Portals.Extivita.Core.Authentication.Services;
using Portals.Extivita.Core.Communication.Repositories;
using Portals.Extivita.Core.Management.Repositories;
using Portals.Extivita.SharedKernel.Communication.Email;
using Portals.Extivita.SharedKernel.Communication.Sms;
using Portals.Extivita.SharedKernel.Domain.Repositories;
using Portals.Extivita.SharedKernel.Jobs;

namespace Portals.Extivita.Core.Appointments.Services.EventHandlers.AppointmentRescheduled
{
    public class NotifyPatientWhenAppointmentRescheduledEvent : INotificationHandler<AppointmentRescheduledEvent>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly ISmsTemplateRepository _smsTemplateRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IUserService _userService;
        private readonly IJobManager _jobManager;
        private readonly IUserPreferenceRepository _preferenceRepository;

        public NotifyPatientWhenAppointmentRescheduledEvent(
            IEmailTemplateRepository emailTemplateRepository,
            ISmsTemplateRepository smsTemplateRepository,
            IAppointmentService appointmentService,
            IRepository<Appointment> appointmentRepository,
            IUserService userService,
            IJobManager jobManager,
            IUserPreferenceRepository preferenceRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _smsTemplateRepository = smsTemplateRepository;
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
            _userService = userService;
            _jobManager = jobManager;
            _preferenceRepository = preferenceRepository;
        }
        public async Task Handle(AppointmentRescheduledEvent domainEvent, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetSingleOrDefaultAsync(
                AppointmentSpecification.ById(domainEvent.AppointmentId));
            var notifiedUser = await _userService.GetResponsibleUser(appointment.Patient.Id);
            var values = await _appointmentService.GetTemplateValues(appointment);
            values["old_date_time"] = domainEvent.OldDateTime.ToString();
            var userPreference = await _preferenceRepository.LoadAsync<CommunicationPreference>(notifiedUser.Id);
            if (userPreference.ReceiveEmailAndCalendarReminders)
            {
                await SendEmail(appointment, notifiedUser, values);
            }
            if (userPreference.ReceiveSmsReminders)
            {
                await SendSms(notifiedUser, values);
            }
        }

        private async Task SendEmail(
            Appointment appointment,
            User notifiedUser,
            Dictionary<string, string> values)
        {
            var template = await _emailTemplateRepository.GetByNameAsync(TemplateNames.AppointmentRescheduled);
            var to = new EmailAddress(notifiedUser.Email, notifiedUser.FullName);
            var message = new EmailMessage("Extivita HBOT Appointment", template, values, to)
            {
                CalendarEntry = _appointmentService.CreateCalendarEntry(appointment, CalendarEntryStatus.Updated)
            };
            _jobManager.Enqueue(message);
        }

        private async Task SendSms(User notifiedUser, Dictionary<string, string> values)
        {
            var template = await _smsTemplateRepository.GetByNameAsync(TemplateNames.AppointmentRescheduled);
            var message = new SmsMessage(template, values, notifiedUser.Address.Phone);
            _jobManager.Enqueue(message);
        }
    }
}