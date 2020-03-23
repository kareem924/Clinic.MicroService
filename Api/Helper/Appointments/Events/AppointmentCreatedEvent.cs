using System;
using MediatR;
using Portals.Extivita.SharedKernel.Domain.Events;

namespace Portals.Extivita.Core.Appointments.Events
{
    public class AppointmentCreatedEvent : DomainEvent
    {
        public Guid AppointmentId { get; }

        public AppointmentCreatedEvent(Guid appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}