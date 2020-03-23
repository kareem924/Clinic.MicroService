using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.General.Entity;

namespace Appointment.Core.Entities
{
    public class TimeSlotItem: GuidIdEntity, IComparable<TimeSlotItem>
    {
        private static readonly TimeSpan MaxTimeOfDay = TimeSpan.FromHours(24);

        public TimeSpan TimeOfDay { get; private set; }

        public DayOfWeek DayOfWeek { get; private set; }

        public TimeSlotItem(DayOfWeek dayOfWeek, TimeSpan timeOfDay)
        {
            if (timeOfDay < TimeSpan.Zero || timeOfDay > MaxTimeOfDay)
            {
                throw new ArgumentOutOfRangeException(nameof(timeOfDay));
            }
            DayOfWeek = dayOfWeek;
            TimeOfDay = timeOfDay;
        }

        public TimeSlotItem(DateTime date)
            : this(date.DayOfWeek, date.TimeOfDay)
        {
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (other.GetType() != this.GetType())
            {
                return false;
            }
            return CompareTo((TimeSlotItem)other) == 0;
        }

        public override int GetHashCode()
        {
            return /*base.GetHashCode() ^*/(int)DayOfWeek ^ TimeOfDay.GetHashCode();
        }

        public int CompareTo(TimeSlotItem other)
        {
            if (other == null)
            {
                return 1;
            }
            return DayOfWeek == other.DayOfWeek
                ? TimeOfDay.CompareTo(other.TimeOfDay)
                : DayOfWeek.CompareTo(other.DayOfWeek);
        }

        public static bool operator <(TimeSlotItem left, TimeSlotItem right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(TimeSlotItem left, TimeSlotItem right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator !=(TimeSlotItem left, TimeSlotItem right)
        {
            return !(left == right);
        }

        public static bool operator ==(TimeSlotItem left, TimeSlotItem right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }
            return left.CompareTo(right) == 0;
        }

        public IEnumerable<DateTime> GetOccurrences(DateTime startDay, DateTime endDay)
        {
            if (endDay < startDay)
            {
                throw new ArgumentOutOfRangeException(nameof(endDay));
            }
            return GetDaysUntil(startDay, endDay)
                .Where(day => day.DayOfWeek == DayOfWeek)
                .Select(day => day + TimeOfDay)
                .Where(dateTime => Between(dateTime, startDay, endDay))
                .ToArray();
        }

        public bool Overlaps(TimeSlotItem other, TimeSpan duration)
        {
            var thisEnd = GetEnd(duration);
            var otherEnd = other.GetEnd(duration);
            if (other < thisEnd && this < otherEnd)
            {
                //Debugger.Break();
            }
            return other < thisEnd && this < otherEnd;
        }

        public override string ToString()
        {
            return $"{DayOfWeek} {TimeOfDay}";
        }

        private TimeSlotItem GetEnd(TimeSpan duration)
        {
            var endTime = TimeOfDay + duration;
            var endDay = DayOfWeek;
            if (endTime >= MaxTimeOfDay)
            {
                endDay = endDay == DayOfWeek.Saturday
                    ? DayOfWeek.Sunday
                    : ++endDay;
                endTime -= MaxTimeOfDay;
            }
            return new TimeSlotItem(endDay, endTime);
        }

        private static IEnumerable<DateTime> GetDaysUntil(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
            for (var day = start.Date; day <= end; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        private static bool Between(DateTime date, DateTime begin, DateTime end)
        {
            return date >= begin && date <= end;
        }
    }
}
