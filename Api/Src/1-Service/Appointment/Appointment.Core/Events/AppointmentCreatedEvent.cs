using System;
using Common.RabbitMq;

namespace Appointment.Core.Events
{
    public class AppointmentCreatedEvent : IntegrationEvent
    {
        public Guid AppointmentId { get; }

        public AppointmentCreatedEvent(Guid appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
