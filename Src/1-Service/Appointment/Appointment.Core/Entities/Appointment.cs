using Appointment.Core.Enums;
using Common.General.Entity;
using Common.General.Interfaces;
using System;


namespace Appointment.Core.Entities
{

    public class Appointment : GuidIdEntity, IAggregateRoot
    {
        public Appointment()
        {

        }
        public AppointmentStatus Status { get; private set; } = AppointmentStatus.NotArrived;

        public string CancelReason { get; private set; }

        public TimeSpan? ArrivalTime { get; private set; }

        public Guid PatientId { get; protected set; }

        public Guid SessionId { get; protected set; }
       

    }
}
