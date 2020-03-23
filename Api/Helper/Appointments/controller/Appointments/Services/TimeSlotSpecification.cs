using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification.Sorting;
using Portals.Extivita.SharedKernel.Helpers;

namespace Portals.Extivita.Core.Appointments.Services
{
    public sealed class TimeSlotSpecification : Specification<TimeSlot>
    {
        private TimeSlotSpecification(Expression<Func<TimeSlot, bool>> criteria) : base(criteria)
        {
            AddIncludes();
        }

        public static TimeSlotSpecification ById(Guid id)
        {
            return new TimeSlotSpecification(timeSlot => timeSlot.Id == id);
        }

        public static TimeSlotSpecification ByChamberId(Guid id)
        {
            return new TimeSlotSpecification(timeSlot => timeSlot.Chamber.Id == id);
        }

        public static ISpecification<TimeSlot> ForTypesWithinDateRange(
            DateTime beginning,
            DateTime end,
            IEnumerable<PrimaryServiceType> typeFilter)
        {
            typeFilter = (typeFilter ?? Enumerable.Empty<PrimaryServiceType>())
                .Distinct()
                .ToArray();
            return new TimeSlotSpecification(WithinDateRange(beginning, end))
                .Where(timeSlot => !typeFilter.Any() || typeFilter.Contains(timeSlot.PrimaryService.Type));
        }

        public static ISpecification<TimeSlot> ByDetails(ISessionDetails details)
        {
            Check.NotNull(details, nameof(details));
            var dateTime = details.DateTime;
            return new TimeSlotSpecification(HasDetails(details))
                .Where(timeSlot =>
                    timeSlot.Items.Any(item =>
                        item.TimeOfDay == dateTime.TimeOfDay &&
                        item.DayOfWeek == dateTime.DayOfWeek))
                .Where(WithinDateRange(dateTime, dateTime));
        }

        public static ISpecification<TimeSlot> ByDetailsOrderByDateDesc(ISessionDetails details)
        {
            Check.NotNull(details, nameof(details));
            var dateTime = details.DateTime;
            return new TimeSlotSpecification(HasDetails(details))
                .Where(timeSlot =>
                    timeSlot.Items.Any(item =>
                        item.TimeOfDay == dateTime.TimeOfDay &&
                        item.DayOfWeek == dateTime.DayOfWeek))
                .Where(WithinDateRange(dateTime, dateTime))
                .AddSorting(timeSlot => timeSlot.CreationDate,SortDirection.Descending);
        }

        public static ISpecification<TimeSlot> ForOverlaps(
            IBasicSessionDetails details, 
            DayOfWeek dayOfWeek,
            TimeSpan timeOfDay,
            TimeSpan duration,
            DateTime startDate,
            DateTime? endDate = null,
            params Guid[] exceptedTimeSlotIds)
        {
            var timeSlotItemTime = timeOfDay + duration;
            return new TimeSlotSpecification(HasDetails(details))
                .Where(timeSlot =>
                    timeSlot.Items.Any(item =>
                        item.DayOfWeek == dayOfWeek &&
                        item.TimeOfDay < timeSlotItemTime &&
                        timeOfDay < item.TimeOfDay + timeSlot.Duration))
                .Where(timeSlot => !exceptedTimeSlotIds.Contains(timeSlot.Id))
                .Where(WithinDateRange(startDate, endDate ?? DateTime.MaxValue));
        }

        public static TimeSlotSpecification ForConsultations()
        {

            return new TimeSlotSpecification(IsConsultation());
        }

        public static TimeSlotSpecification ForDives()
        {
            return new TimeSlotSpecification(IsDive());
        }

        public static TimeSlotSpecification ForOrientations()
        {
            return new TimeSlotSpecification(IsOrientation());
        }

        //public static TimeSlotSpecification ForDiveById(Guid id)
        //{
        //    var result = new TimeSlotSpecification(timeSlot => timeSlot.Id == id);
        //    result.Where(IsDive());
        //    return result;
        //}

        private static Expression<Func<TimeSlot, bool>> IsDive() => slot => slot.Chamber != null;

        private static Expression<Func<TimeSlot, bool>> IsConsultation() => timeSlot => timeSlot.Room != null;

        private static Expression<Func<TimeSlot, bool>> IsOrientation() =>
            timeSlot => timeSlot.Room == null &&
                        timeSlot.Chamber == null;

        private static Expression<Func<TimeSlot, bool>> HasDetails(IBasicSessionDetails details)
        {
            return timeSlot =>
                timeSlot.PrimaryService.Type == details.PrimaryServiceType &&
                timeSlot.Chamber.Id == details.ChamberId &&
                timeSlot.Room.Id == details.RoomId &&
                timeSlot.Employee.Id == details.EmployeeId;
        }

        private static Expression<Func<TimeSlot, bool>> WithinDateRange(DateTime beginning, DateTime end)
        {
            return timeSlot =>
                timeSlot.StartDate <= end &&
                beginning <= (timeSlot.EndDate ?? DateTime.MaxValue);
        }

        private void AddIncludes()
        {
            Include(timeSlot => timeSlot.PrimaryService.AdditionalServices)
                .Include(timeSlot => timeSlot.Items)
                .Include(timeSlot => timeSlot.Chamber.Seats)
                .Include(timeSlot => timeSlot.Room)
                .Include(timeSlot => timeSlot.ChamberOperator)
                .Include(timeSlot => timeSlot.BackupChamberOperator)
                .Include(timeSlot => timeSlot.InsideAttendant)
                .Include(timeSlot => timeSlot.BackupInsideAttendant)
                .Include(timeSlot => timeSlot.Employee.UserRoles)
                        .ThenInclude(userRole => userRole.Role);
        }
    }
}