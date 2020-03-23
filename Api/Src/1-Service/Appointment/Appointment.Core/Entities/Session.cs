using Common.General.Entity;
using Common.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Appointment.Core.Entities
{
    public class Session : GuidIdEntity, IHaveSessionDetails, IAggregateRoot
    {
        private readonly List<Appointment> _appointments = new List<Appointment>();

        public ServiceType Service { get; private set; }

        public SessionStatus Status { get; private set; } = SessionStatus.Default;

        public DateTime DateTime { get; private set; }

        public TimeSpan Duration { get; private set; }

        public int ReferenceNumber { get; private set; }

        public string ReferenceTimeSlotId { get; set; }

        public Guid ConsultantId { get; private set; }

        public string CancelReason { get; private set; }

        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        public IReadOnlyCollection<Appointment> NotCanceledAppointments =>
            _appointments.Where(appointment => appointment.Status != Enums.AppointmentStatus.Canceled).ToArray();



        private Session()
        {
        }

        public Session(SessionStatus status, DateTime dateTime, TimeSlot timeSlot)
        {
            Status = status;
            DateTime = dateTime;
            CopyTimeSlotDetails(timeSlot);
        }

        public void Book(
            [NotNull] Appointment appointment)
        {
            ValidateNotCanceled();
            if (DateTime.Now > DateTime.AddDays(1).AddTicks(-1))
            {
                throw new InvalidOperationException("Can't book in outdated session.");
            }
            var appointmentExists = NotCanceledAppointments.Any(a => a.PatientId == appointment.PatientId);
            if (appointmentExists)
            {
                throw new Exception("patient already has an appointment at that session.");
            }
            _appointments.Add(appointment);
        }

        public void RescheduleAppointment(
            Appointment appointment)
        {
            ValidateNotCanceled();
            if (!NotCanceledAppointments.Contains(appointment))
            {
                throw new InvalidOperationException("Appointment not found.");
            }

            _appointments.Remove(appointment);
            //toTherapySession.Book(appointment, seats);
            appointment.Reschedule(DateTime);
        }

        public void Reschedule(DateTime newDateTime)
        {
            ValidateNotCanceled();
            if (newDateTime < DateTime.Now)
            {
                throw new InvalidOperationException("Can't reschedule to past time.");
            }
            var oldDateTime = DateTime;
            DateTime = newDateTime;
            foreach (var appointment in NotCanceledAppointments)
            {
                appointment.Reschedule(oldDateTime);
            }
        }

        public void Cancel(string reason, bool refundPatients)
        {
            ValidateNotCanceled();
            Status = SessionStatus.Canceled;
            CancelReason = reason;
            foreach (var appointment in NotCanceledAppointments)
            {
                appointment.Cancel(reason, refundPatients);
            }
        }

        public bool IsConsultation()
        {
            return Service == ServiceType.Consultation;
        }

        public bool IsPhoneConsultation()
        {
            return Service == ServiceType.PhoneConsultation;
        }

        private void CopyTimeSlotDetails(TimeSlot timeSlot)
        {
            //TODO : update this data
            //PrimaryService = timeSlot.PrimaryService;
            //Duration = timeSlot.Duration;
            //ConsultantId = timeSlot.Employee;
            //ReferenceTimeSlotId = timeSlot.Id.ToString();

        }

        private void ValidateNotCanceled()
        {
            if (Status == SessionStatus.Canceled)
            {
                throw new InvalidOperationException("Can't take action on canceled Therapy Session.");
            }
        }


    }
}
