using System;
using System.Collections.Generic;
using System.Text;
using Portals.Extivita.SharedKernel.Domain.Events;

namespace Portals.Extivita.Core.Appointments.Events
{
    public class AppointmentCanceledEvent : DomainEvent
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
