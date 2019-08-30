using Appointment.Core.Enums;
using Common.General.Entity;
using Common.General.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appointment.Core.Entities
{
    public class Session : FullTrackInfoEntity<ObjectId>, IAggregateRoot
    {
        public Session(SessionStatus status, DateTime dateTime, TimeSpan duration)
        {
            Status = status;
            DateTime = dateTime;
            Duration = duration;
        }

        private readonly List<Appointment> _appointments = new List<Appointment>();

        public PrimaryServiceType PrimaryService { get; private set; }

        public SessionStatus Status { get; private set; } = SessionStatus.Default;

        public DateTime DateTime { get; private set; }

        public TimeSpan Duration { get; private set; }

        //public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        //public IReadOnlyCollection<Appointment> NotCanceledAppointments =>
        //    _appointments.Where(appointment => appointment.Status != AppointmentStatus.Canceled).ToArray();
    }
}
