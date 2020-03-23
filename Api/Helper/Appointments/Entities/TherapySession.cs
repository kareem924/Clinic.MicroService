using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Portals.Extivita.Core.Appointments.Entities.SessionChecklists;
using Portals.Extivita.Core.Appointments.Events;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.SharedKernel.Domain.Entities;
using Portals.Extivita.SharedKernel.Exceptions;
using Portals.Extivita.SharedKernel.Extensions;
using Portals.Extivita.SharedKernel.Helpers;
using Portals.Extivita.SharedKernel.Timing;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class TherapySession : FullAuditedEntity, IHaveSessionDetails, IHaveAssignedStuff
    {
        private readonly List<Appointment> _appointments = new List<Appointment>();

        public PrimaryService PrimaryService { get; private set; }

        public TherapySessionStatus Status { get; private set; } = TherapySessionStatus.Default;

        public DateTime DateTime { get; private set; }

        public TimeSpan Duration { get; private set; }

        public string BreathingGas { get; private set; }

        public string PressureATA { get; private set; }

        public int ReferenceNumber { get; private set; }

        public string ReferenceTimeSlotId { get; set; }

        public Chamber Chamber { get; private set; }

        public Room Room { get; private set; }

        public User Employee { get; private set; }

        public User ChamberOperator { get; private set; }

        public User BackupChamberOperator { get; private set; }

        public User InsideAttendant { get; private set; }

        public User BackupInsideAttendant { get; private set; }

        public string CancelReason { get; private set; }

        public ChamberOperatorChecklist ChamberOperatorChecklist { get; private set; }

        public PreDiveChecklist PreDiveChecklist { get; private set; }

        public DiveNoteChecklist DiveNoteChecklist { get; private set; }

        public InsideAttendantChecklist InsideAttendantChecklist { get; private set; }

        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        public IReadOnlyCollection<Appointment> NotCanceledAppointments =>
            _appointments.Where(appointment => appointment.Status != Enums.AppointmentStatus.Canceled).ToArray();

        public HoldInfo HoldInfo { get; private set; } = new HoldInfo();

        public bool IsOccupied
        {
            get
            {
                var maxAppointmentsCount = PrimaryService.Type.IsDive()
                    ? Chamber.Seats.Count
                    : 1;
                return NotCanceledAppointments.Count >= maxAppointmentsCount;
            }
        }

        public IReadOnlyCollection<ChamberSeat> AvailableSeats
        {
            get
            {
                if (!PrimaryService.Type.IsDive())
                {
                    return new ChamberSeat[0];
                }
                var occupiedSeats = NotCanceledAppointments.SelectMany(appointment => appointment.Seats);
                return Chamber.Seats
                    .Except(occupiedSeats)
                    .ToArray();
            }
        }

        private TherapySession()
        {
        }

        public TherapySession(DateTime dateTime, TimeSlot timeSlot)
        {
            DateTime = dateTime;
            CopyTimeSlotDetails(timeSlot);
        }

        public void Book(
            [NotNull] Appointment appointment,
            IReadOnlyCollection<ChamberSeat> seats,
            IEnumerable<AdditionalService> newAdditionalService = null)
        {
            newAdditionalService = (newAdditionalService ?? new AdditionalService[0]);
            ValidateNotCanceled();
            if (Clock.Now > DateTime.GetEndOfDay())
            {
                throw new InvalidOperationException("Can't book in outdated session.");
            }
            var appointmentExists = NotCanceledAppointments.Any(a => a.Patient == appointment.Patient);
            if (appointmentExists)
            {
                throw new UserException("patient already has an appointment at that session.");
            }
            if (IsOccupied)
            {
                throw new UserException("This session is taken.");
            }
            if (PrimaryService.Type.IsDive())
            {
                Check.NotEmpty(seats.ToArray(), "Seats can't be empty for dive.");
                var appointmentSeatsAvailable = AvailableSeats.Contains(seats);
                if (!appointmentSeatsAvailable)
                {
                    throw new UserException("Selected seats are not available.");
                }
            }
            if (seats.Count() > 1)
            {
                newAdditionalService = newAdditionalService.Append(GetExtraSeatAdditionalService());
            }
            appointment.SetAdditionalServices(newAdditionalService);
            appointment.SetSeats(seats);
            _appointments.Add(appointment);
        }

        public void RescheduleAppointment(
            Appointment appointment,
            TherapySession toTherapySession,
            IReadOnlyCollection<ChamberSeat> seats)
        {
            ValidateNotCanceled();
            if (!NotCanceledAppointments.Contains(appointment))
            {
                throw new InvalidOperationException("Appointment not found.");
            }
            if (appointment.Seats.Count() != seats.Count())
            {
                throw new InvalidOperationException("Rescheduled seats count must match.");
            }
            _appointments.Remove(appointment);
            toTherapySession.Book(appointment, seats);
            appointment.Reschedule(DateTime);
        }

        public void Reschedule(DateTime newDateTime)
        {
            ValidateNotCanceled();
            if (newDateTime < Clock.Now)
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

        public void SetAssignedStuff(
            User chamberOperator,
            User backupChamberOperator,
            User insideAttendant,
            User backupInsideAttendant)
        {
            ChamberOperator = chamberOperator;
            BackupChamberOperator = backupChamberOperator;
            InsideAttendant = insideAttendant;
            BackupInsideAttendant = backupInsideAttendant;
        }

        public void Cancel(string reason, bool refundPatients)
        {
            ValidateNotCanceled();
            Status = TherapySessionStatus.Canceled;
            CancelReason = reason;
            foreach (var appointment in NotCanceledAppointments)
            {
                appointment.Cancel(reason, refundPatients);
            }
        }

        public IEnumerable<ChamberSeat> FindSeatsByIds(IEnumerable<Guid> seatIds)
        {
            var result = Chamber?.Seats
                .Where(seat => seatIds.Contains(seat.Id))
                .ToArray()
                ?? Enumerable.Empty<ChamberSeat>();
            Check.ShouldBe(result.Count(), seatIds.Count(), nameof(seatIds), "Not all seats found in chamber.");
            return result;
        }

        public IEnumerable<AdditionalService> FindAdditionalServiceByIds(IEnumerable<Guid> additionalServiceIds)
        {
            var result = PrimaryService.AdditionalServices
                .Where(additionalService => additionalServiceIds.Contains(additionalService.Id))
                .ToArray();
            var errorMessage = $"Not all additional services found in {PrimaryService.Type} .";
            Check.ShouldBe(result.Count(), additionalServiceIds.Count(), nameof(additionalServiceIds), errorMessage);
            return result;
        }

        public AdditionalService GetExtraSeatAdditionalService()
        {
            return PrimaryService.AdditionalServices
                .Single(additionalService => additionalService.Name.Contains("Extra seat"));
        }

        public void FillChecklist(ChamberOperatorChecklist checklist)
        {
            ChamberOperatorChecklist = checklist;
        }

        public void FillChecklist(DiveNoteChecklist checklist)
        {
            DiveNoteChecklist = checklist;
        }

        public void FillChecklist(InsideAttendantChecklist checklist)
        {
            InsideAttendantChecklist = checklist;
        }

        public void FillChecklist(PreDiveChecklist checklist)
        {
            PreDiveChecklist = checklist;
        }

        // TODO: move to value object
        public void Hold(string heldFor, int holdingPeriod = 5)
        {
            ValidateNotCanceled();
            HoldInfo.HeldFor = heldFor;
            HoldInfo.HeldTo = Clock.Now.AddMinutes(holdingPeriod);
        }

        private void CopyTimeSlotDetails(TimeSlot timeSlot)
        {
            PrimaryService = timeSlot.PrimaryService;
            Duration = timeSlot.Duration;
            BreathingGas = timeSlot.BreathingGas;
            PressureATA = timeSlot.PressureATA;
            Chamber = timeSlot.Chamber;
            Room = timeSlot.Room;
            Employee = timeSlot.Employee;
            ReferenceTimeSlotId = timeSlot.Id.ToString();
            SetAssignedStuff(
                chamberOperator: timeSlot.ChamberOperator,
                backupChamberOperator: timeSlot.BackupInsideAttendant,
                insideAttendant: timeSlot.InsideAttendant,
                backupInsideAttendant: timeSlot.BackupInsideAttendant);
        }

        private void ValidateNotCanceled()
        {
            if (Status == TherapySessionStatus.Canceled)
            {
                throw new InvalidOperationException("Can't take action on canceled Therapy Session.");
            }
        }
    }
}