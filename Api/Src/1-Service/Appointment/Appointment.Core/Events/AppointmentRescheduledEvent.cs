using System;
using System.Collections.Generic;
using System.Text;
using Common.RabbitMq;

namespace Appointment.Core.Events
{
    public class AppointmentRescheduledEvent : IntegrationEvent
    {
        public Guid AppointmentId { get; }

        public DateTime OldDateTime { get; }

        public AppointmentRescheduledEvent(Guid appointmentId, DateTime oldDateTime)
        {
            AppointmentId = appointmentId;
            OldDateTime = oldDateTime;
        }
    }
}
