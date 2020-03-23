using System;
using System.Collections.Generic;
using System.Linq;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.SharedKernel.Domain.Entities;
using Portals.Extivita.SharedKernel.Domain.Entities.Protection;
using Portals.Extivita.SharedKernel.Domain.ValueObject;
using Portals.Extivita.SharedKernel.Helpers;
using Portals.Extivita.SharedKernel.Timing;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class TimeSlot : FullAuditedEntity, IHaveSessionDetails, IHaveAssignedStuff
    {
        private readonly List<TimeSlotItem> _items = new List<TimeSlotItem>();

        public PrimaryService PrimaryService { get; private set; }

        public TimeSpan Duration { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public IReadOnlyCollection<TimeSlotItem> Items => _items.AsReadOnly();

        public string BreathingGas { get; private set; }

        public string PressureATA { get; private set; }

        public Chamber Chamber { get; private set; }

        public Room Room { get; private set; }

        public User Employee { get; private set; }

        public User ChamberOperator { get; private set; }

        public User BackupChamberOperator { get; private set; }

        public User InsideAttendant { get; private set; }

        public User BackupInsideAttendant { get; private set; }

        //public AssignedStuff AssignedStuff { get; set; }

        private TimeSlot()
        {
        }

        private TimeSlot(PrimaryService primaryService)
        {
            PrimaryService = primaryService ?? throw new ArgumentNullException(nameof(primaryService));
        }

        public static TimeSlot NewDive(
            PrimaryService primaryService,
            Chamber chamber,
            string breathingGas,
            string pressureATA,
            TimeSpan duration,
            DateTime startDate,
            DateTime? endDate = null)
        {

            var timeSlot = new TimeSlot(primaryService);
            timeSlot.SetDateTimes(duration, startDate, endDate);
            timeSlot.SetDiveDetails(breathingGas, pressureATA, chamber);

            return timeSlot;
        }

        public static TimeSlot NewConsultation(
            PrimaryService primaryService,
            Room room,
            User employee,
            TimeSpan duration,
            DateTime startDate,
            DateTime? endDate = null)
        {
            var timeSlot = new TimeSlot(primaryService);
            timeSlot.SetDateTimes(duration, startDate, endDate);
            timeSlot.SetConsultationDetails(room, employee);

            return timeSlot;
        }

        public static TimeSlot NewPhoneConsultation(
            PrimaryService primaryService,
            User employee,
            TimeSpan duration,
            DateTime startDate,
            DateTime? endDate = null)
        {
            var timeSlot = new TimeSlot(primaryService);
            timeSlot.SetPhoneConsultationDetails(employee);
            timeSlot.SetDateTimes(duration, startDate, endDate);

            return timeSlot;
        }

        public static TimeSlot NewOrientation(
            PrimaryService primaryService,
            TimeSpan duration,
            DateTime startDate,
            DateTime? endDate = null)
        {
            primaryService.Type.ShouldBe(PrimaryServiceType.Orientation, nameof(primaryService));
            var timeSlot = new TimeSlot(primaryService);
            timeSlot.SetDateTimes(duration, startDate, endDate);

            return timeSlot;
        }

        public IReadOnlyCollection<DateTime> GetOccurrences(DateTime beginning, DateTime end)
        {
            if (end < beginning)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
            beginning = DateTimeHelper.Max(beginning, StartDate);
            end = DateTimeHelper.Min(end, EndDate ?? DateTime.MaxValue);
            if (end < beginning)
            {
                return new DateTime[0];
            }
            return Items.SelectMany(item => item.GetOccurrences(beginning, end)).OrderBy(date => date).ToArray();
        }

        public void SetItems(IEnumerable<TimeSlotItem> items)
        {
            ValidateItems(items);
            _items.Clear();
            _items.AddRange(items);
        }

        public void SetDateTimes(TimeSpan duration, DateTime startDate, DateTime? endDate)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentException("duration should be > 0.", nameof(duration));
            }
            if (endDate < startDate)
            {
                throw new ArgumentOutOfRangeException(nameof(endDate));
            }
            if (endDate <= Clock.Now)
            {
                throw new ArgumentException("endDate should not be in past.", nameof(endDate));
            }
            Duration = duration;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void SetDiveDetails(string breathingGas, string pressureATA, Chamber chamber)
        {
            PrimaryService.Type.ShouldBe(PrimaryServiceType.Dive, nameof(PrimaryService));
            if (string.IsNullOrWhiteSpace(breathingGas))
            {
                throw new ArgumentException("breathingGas should have value.", nameof(breathingGas));
            }
            if (string.IsNullOrWhiteSpace(pressureATA))
            {
                throw new ArgumentException("pressureATA should have value.", nameof(pressureATA));
            }
            BreathingGas = breathingGas;
            PressureATA = pressureATA;
            Chamber = chamber;
        }

        public void SetConsultationDetails(Room room, User employee)
        {
            PrimaryService.Type.ShouldBe(PrimaryServiceType.Consultation, nameof(PrimaryService));
            Room = room ?? throw new ArgumentNullException(nameof(room));
            Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }

        public void SetPhoneConsultationDetails(User employee)
        {
            PrimaryService.Type.ShouldBe(PrimaryServiceType.PhoneConsultation, nameof(PrimaryService));
            Employee = employee ?? throw new ArgumentNullException(nameof(employee));
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

        public override string ToString()
        {
            return $"TimeSlot{{{PrimaryService}, Items={Items.Count}}}";
        }

        private void ValidateItems(IEnumerable<TimeSlotItem> items)
        {
            items = items as TimeSlotItem[] ?? items.ToArray();
            var overlaps = items.SelectMany(item =>
                items.Except(new[] { item })
                    .Where(other => item.Overlaps(other, Duration)));
            if (overlaps.Any() || items.Distinct().Count() != items.Count())
            {
                throw new ArgumentException("time-slot items should not overlap.");
            }
        }
    }

    public interface IHaveAssignedStuff
    {
        User ChamberOperator { get; }

        User BackupChamberOperator { get; }

        User InsideAttendant { get; }

        User BackupInsideAttendant { get; }

        void SetAssignedStuff(
             User chamberOperator,
            User backupChamberOperator,
            User insideAttendant,
            User backupInsideAttendant);
    }
}

