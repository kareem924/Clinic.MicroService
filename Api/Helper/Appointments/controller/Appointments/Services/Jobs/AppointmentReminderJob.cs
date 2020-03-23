using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.Authentication.Preferences;
using Portals.Extivita.Core.Authentication.Services;
using Portals.Extivita.Core.Communication.Repositories;
using Portals.Extivita.Core.Management.Repositories;
using Portals.Extivita.SharedKernel.Communication.Email;
using Portals.Extivita.SharedKernel.Communication.Sms;
using Portals.Extivita.SharedKernel.Domain.Repositories;
using Portals.Extivita.SharedKernel.Jobs;
using Portals.Extivita.SharedKernel.Jobs.Attributes;
using Portals.Extivita.SharedKernel.Timing;

namespace Portals.Extivita.Core.Appointments.Services.Jobs
{
    [DailyJob(EveryDay, Noon)]
    internal class AppointmentReminderJob : IPeriodicJob
    {
        private const int EveryDay = 1;
        private const int Noon = 12;
        private readonly IAppointmentService _appointmentService;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly ISmsTemplateRepository _smsTemplateRepository;
        private readonly IUserService _userService;
        private readonly IJobManager _jobManager;
        private readonly ILogger<AppointmentReminderJob> _logger;
        private readonly IUserPreferenceRepository _preferenceRepository;

        public AppointmentReminderJob(
            IAppointmentService appointmentService,
            IRepository<Appointment> appointmentRepository,
            IEmailTemplateRepository emailTemplateRepository,
            ISmsTemplateRepository smsTemplateRepository,
            IUserService userService,
            IJobManager jobManager,
            ILogger<AppointmentReminderJob> logger,
            IUserPreferenceRepository preferenceRepository)
        {
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _smsTemplateRepository = smsTemplateRepository;
            _userService = userService;
            _jobManager = jobManager;
            _logger = logger;
            _preferenceRepository = preferenceRepository;
        }

        public async Task RunAsync()
        {
            _logger.LogDebug("Appointment Reminder job started.");
            var appointments = await GetTomorrowAppointments();
            _logger.LogDebug($"Found appointments {appointments.Count}.");
            foreach (var appointment in appointments)
            {
                var notifiedUser = await _userService.GetResponsibleUser(appointment.Patient.Id);
                await SendReminder(appointment, notifiedUser);
                _logger.LogInformation(
                    $"Sending reminder for appointment {appointment.Id} to patient {notifiedUser.FullName}");
            }
        }

        private async Task SendReminder(Appointment appointment, User notifiedUser)
        {
            var userPreference = await _preferenceRepository.LoadAsync<CommunicationPreference>(notifiedUser.Id);
            if (userPreference.ReceiveEmailAndCalendarReminders)
            {
                await SendReminderEmail(appointment, notifiedUser);
            }
            if (userPreference.ReceiveSmsReminders)
            {
                await SendReminderSms(appointment, notifiedUser);
            }
        }

        private Task<IReadOnlyCollection<Appointment>> GetTomorrowAppointments()
        {
            var tomorrow = Clock.Today.AddDays(1);
            return _appointmentRepository.GetAllAsync(
                AppointmentSpecification.ByDay(tomorrow).NotCanceled());
        }

        private async Task SendReminderEmail(Appointment appointment, User notifiedUser)
        {
            var template = await _emailTemplateRepository.GetByNameAsync(TemplateNames.AppointmentReminder);
            var values = await _appointmentService.GetTemplateValues(appointment);
            var to = new EmailAddress(notifiedUser.Email, notifiedUser.FullName);
            var message = new EmailMessage("Appointment Reminder", template, values, to);
            //{
            //    CalendarEntry = _appointmentService.CreateCalendarEntry(appointment, CalendarEntryStatus.Confirmed)
            //};
            _jobManager.Enqueue(message);
        }

        private async Task SendReminderSms(Appointment appointment, User notifiedUser)
        {
            var template = await _smsTemplateRepository.GetByNameAsync(TemplateNames.AppointmentReminder);
            var values = await _appointmentService.GetTemplateValues(appointment);
            var message = new SmsMessage(template, values, notifiedUser.Address.Phone);
            _jobManager.Enqueue(message);
        }
    }
}