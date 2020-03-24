using System;
using System.Collections.Generic;
using System.Text;
using Common.RabbitMq;

namespace Appointment.Core.Events
{
    public class AppointmentCanceledEvent : IntegrationEvent
    {
        public Guid AppointmentId { get; }

        public bool RefundPayment { get; }

        public AppointmentCanceledEvent(Guid appointmentId, bool refundPayment)
        {
            AppointmentId = appointmentId;
            RefundPayment = refundPayment;
        }
    }
}
