using System.Threading;
using System.Threading.Tasks;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.Appointments.Events;
using Portals.Extivita.Core.Payments.Entities;
using Portals.Extivita.Core.Payments.Services;
using Portals.Extivita.SharedKernel.Domain.Events;
using Portals.Extivita.SharedKernel.Domain.Repositories;

namespace Portals.Extivita.Core.Appointments.Services.EventHandlers.AppointmentCanceled
{
    internal class RefundCreditsWhenAppointmentCanceledEventHandler : IEventHandler<AppointmentCanceledEvent>
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IRepository<Appointment> _appointmentRepository;

        public RefundCreditsWhenAppointmentCanceledEventHandler(
            ICheckoutService checkoutService,
            IRepository<Appointment> appointmentRepository)
        {
            _checkoutService = checkoutService;
            _appointmentRepository = appointmentRepository;
        }

        public async Task Handle(AppointmentCanceledEvent domainEvent, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetSingleOrDefaultAsync(
                AppointmentSpecification.ById(domainEvent.AppointmentId));
            if (domainEvent.RefundPayment && appointment.Transaction?.Status == TransactionStatus.Completed)
            {
                await _checkoutService.RefundAsync(appointment.Transaction, RefundType.PatientCredit, appointment.Transaction.Amount);
            }
        }
    }
}
