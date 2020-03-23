using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
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

namespace Portals.Extivita.Core.Appointments.Services.EventHandlers
{
    internal class SendConfirmationWhenAppointmentCreatedEventHandler
        : INotificationHandler<AppointmentCreatedEvent>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly ISmsTemplateRepository _smsTemplateRepository;
        private readonly IUserService _userService;
        private readonly IJobManager _jobManager;
        private readonly ILogger<SendConfirmationWhenAppointmentCreatedEventHandler> _logger;
        private readonly IUserPreferenceRepository _preferenceRepository;

        public SendConfirmationWhenAppointmentCreatedEventHandler(
            IAppointmentService appointmentService,
            IRepository<Appointment> appointmentRepository,
            IEmailTemplateRepository emailTemplateRepository,
            ISmsTemplateRepository smsTemplateRepository,
            IUserService userService,
            IJobManager jobManager,
            ILogger<SendConfirmationWhenAppointmentCreatedEventHandler> logger,
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

        public async Task Handle(AppointmentCreatedEvent appointmentCreatedEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{appointmentCreatedEvent}", appointmentCreatedEvent);
            var appointment = await _appointmentRepository.GetFirstOrDefaultAsync(
                AppointmentSpecification.ById(appointmentCreatedEvent.AppointmentId));
            var notifiedUser = await _userService.GetResponsibleUser(appointment.Patient.Id);
            _logger.LogInformation($"Sending confirmation message to patient {notifiedUser.FullName}");
            var userPreference = await _preferenceRepository.LoadAsync<CommunicationPreference>(notifiedUser.Id);
            if (userPreference.ReceiveEmailAndCalendarReminders)
            {
                await SendEmail(appointment, notifiedUser);
            }
            if (userPreference.ReceiveSmsReminders)
            {
                await SendSms(appointment, notifiedUser);
            }
        }

        private async Task SendSms(Appointment appointment, User notifiedUser)
        {
            var template = await _smsTemplateRepository.GetByNameAsync(TemplateNames.ConfirmAppointment);
            var values = await _appointmentService.GetTemplateValues(appointment);
            var message = new SmsMessage(template, values, notifiedUser.Address.Phone);
            _jobManager.Enqueue(message);
        }

        private async Task SendEmail(Appointment appointment, User notifiedUser)
        {
            var template = await _emailTemplateRepository.GetByNameAsync(TemplateNames.ConfirmAppointment);
            var values = await _appointmentService.GetTemplateValues(appointment);
            var to = new EmailAddress(notifiedUser.Email, notifiedUser.FullName);
            var message = new EmailMessage("Extivita HBOT Appointment", template, values, to)
            {
                CalendarEntry = _appointmentService.CreateCalendarEntry(appointment, CalendarEntryStatus.Confirmed)
            };
            _jobManager.Enqueue(message);
        }
    }
}