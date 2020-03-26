using System;
using System.Collections.Generic;
using System.Linq;
using Common.General.Entity;

namespace Appointment.Core.Entities
{
    public class TimeSlot : GuidIdEntity
    {
        private readonly List<TimeSlotItem> _items = new List<TimeSlotItem>();

        public ServiceType Service { get; private set; }

        public TimeSpan Duration { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public IReadOnlyCollection<TimeSlotItem> Items => _items.AsReadOnly();

        public Guid ConsultantId { get; private set; }

        private TimeSlot()
        {
        }

        private TimeSlot(ServiceType service)
        {
            Service = service;
        }


        public static TimeSlot NewConsultation(
            Guid consultantId,
            TimeSpan duration,
            DateTime startDate,
            DateTime? endDate = null)
        {
            var timeSlot = new TimeSlot(ServiceType.Consultation);
            timeSlot.SetDateTimes(duration, startDate, endDate);
            timeSlot.SetConsultationDetails(consultantId);

            return timeSlot;
        }

        public static TimeSlot NewPhoneConsultation(
            Guid consultantId,
            TimeSpan duration,
            DateTime startDate,
            DateTime? endDate = null)
        {
            var timeSlot = new TimeSlot(ServiceType.PhoneConsultation);
            timeSlot.SetPhoneConsultationDetails(consultantId);
            timeSlot.SetDateTimes(duration, startDate, endDate);

            return timeSlot;
        }

        public IReadOnlyCollection<DateTime> GetOccurrences(DateTime beginning, DateTime end)
        {
            if (end < beginning)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
            beginning = Max(beginning, StartDate);
            end = Min(end, EndDate ?? DateTime.MaxValue);
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
            if (endDate <= DateTime.Now)
            {
                throw new ArgumentException("endDate should not be in past.", nameof(endDate));
            }
            Duration = duration;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void SetConsultationDetails(Guid consultantId)
        {
            ConsultantId = consultantId;
        }

        public void SetPhoneConsultationDetails(Guid consultantId)
        {
            ConsultantId = consultantId;
        }



        public override string ToString()
        {
            return $"TimeSlot{{{Service}, Items={Items.Count}}}";
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

        private DateTime Min(DateTime left, DateTime right)
        {
            return new DateTime(Math.Min(left.Ticks, right.Ticks));
        }

        private DateTime Max(DateTime left, DateTime right)
        {
            return new DateTime(Math.Max(left.Ticks, right.Ticks));
        }
    }
}