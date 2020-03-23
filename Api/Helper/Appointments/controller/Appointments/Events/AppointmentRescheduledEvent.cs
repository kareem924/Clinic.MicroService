using System;
using Portals.Extivita.SharedKernel.Domain.Events;

namespace Portals.Extivita.Core.Appointments.Events
{
    public class AppointmentRescheduledEvent : DomainEvent
    {
        public Guid AppointmentId { get; }

        public DateTime OldDateTime { get; }

        //TODO: need to pass sequence[version] of the appointment, issue will happen in future reschedules
        public AppointmentRescheduledEvent(Guid appointmentId, DateTime oldDateTime)
        {
            AppointmentId = appointmentId;
            OldDateTime = oldDateTime;
        }
    }
}