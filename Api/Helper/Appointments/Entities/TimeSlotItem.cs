using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Portals.Extivita.SharedKernel.Domain.Entities;
using Portals.Extivita.SharedKernel.Helpers;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class TimeSlotItem : AuditedEntity, IComparable<TimeSlotItem>
    {
        private static readonly TimeSpan _maxTimeOfDay =TimeSpan.FromHours(24);

        public TimeSpan TimeOfDay { get; private set; }

        public DayOfWeek DayOfWeek { get; private set; }

        public TimeSlotItem(DayOfWeek dayOfWeek, TimeSpan timeOfDay)
        {
            if (timeOfDay < TimeSpan.Zero || timeOfDay > _maxTimeOfDay)
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
            return startDay.GetDaysUntil(endDay)
                .Where(day => day.DayOfWeek == DayOfWeek)
                .Select(day => day + TimeOfDay)
                .Where(dateTime => dateTime.Between(startDay, endDay))
                .ToArray();
        }

        public bool Overlaps(TimeSlotItem other, TimeSpan duration)
        {
            var thisEnd = GetEnd(duration);
            var otherEnd = other.GetEnd(duration);
            if (other < thisEnd && this < otherEnd)
            {
                Debugger.Break();
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
            if (endTime >= _maxTimeOfDay)
            {
                endDay = endDay.GetNextDay();
                endTime -= _maxTimeOfDay;
            }
            return new TimeSlotItem(endDay, endTime);
        }
    }
}

