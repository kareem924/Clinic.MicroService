using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification.Sorting;
using Portals.Extivita.SharedKernel.Timing;

namespace Portals.Extivita.Core.Appointments.Services
{
    public class TherapySessionSpecification : Specification<TherapySession>
    {
        private TherapySessionSpecification(Expression<Func<TherapySession, bool>> criteria)
            : base(criteria)
        {
            AddIncludes();
        }

        public static TherapySessionSpecification ById(Guid id)
        {
            return new TherapySessionSpecification(therapySession => therapySession.Id == id);
        }

        public static TherapySessionSpecification ByAppointmentId(Guid id)
        {
            return new TherapySessionSpecification(therapySession =>
                therapySession.Appointments.Any(appointment => appointment.Id == id));
        }

        public static TherapySessionSpecification ByChamberId(Guid id)
        {
            return new TherapySessionSpecification(therapySession => therapySession.Chamber.Id == id);
        }


        public static ISpecification<TherapySession> ByDetails(ISessionDetails details)
        {
            // get latest by date always
            return new TherapySessionSpecification(HasDetails(details))
                .Where(therapySession =>
                    therapySession.DateTime == details.DateTime &&
                    therapySession.Status == TherapySessionStatus.Default);
        }

        public static ISpecification<TherapySession> ByDetailsOrderDesc(ISessionDetails details)
        {
            // to be removed
            return ByDetails(details)
                .AddSorting(therapySession => therapySession.CreationDate, SortDirection.Descending);
        }

#if false
        public static ISpecification<TherapySession> ForOverlaps(IBasicSessionDetails details,
            DayOfWeek dayOfWeek,
            TimeSpan timeOfDay,
            TimeSpan duration)
        {
            // Issue: this is not working code , DateTime.DayOfWeek (for datetime-column) cannot be translated to sql
            // and will throw exception
            var endTime = timeOfDay + duration;
            return new Specification<TherapySession>(HasDetails(details))
                .Where(therapySession => therapySession.DateTime.DayOfWeek == dayOfWeek)
                .Where(therapySession =>
                    therapySession.DateTime.TimeOfDay < endTime &&
                    timeOfDay < therapySession.DateTime.TimeOfDay + therapySession.Duration);
        } 
#endif

        public static TherapySessionSpecification ByHeldForId(string id)
        {
            return new TherapySessionSpecification(therapySession => therapySession.HoldInfo.HeldFor == id);
        }

        public static TherapySessionSpecification ForOnHoldSessionsByHeldForId(string id)
        {
            return new TherapySessionSpecification(therapySession =>
              therapySession.HoldInfo.HeldTo.HasValue &&
              therapySession.HoldInfo.HeldTo > Clock.Now &&
              therapySession.HoldInfo.HeldFor == id &&
              !therapySession.Appointments.Any());
        }

        public static TherapySessionSpecification ForConsultationsWithinDateRange(DateTime beginning, DateTime end)
        {
            var result = new TherapySessionSpecification(IsConsultation());
            result.Where(WithinRange(beginning, end));
            return result;
        }

        public static TherapySessionSpecification ForDivesWithinDateRange(DateTime beginning, DateTime end)
        {
            var result = new TherapySessionSpecification(IsDive());
            result.Where(WithinRange(beginning, end));
            return result;
        }

        public static TherapySessionSpecification ForOrientationWithinDateRange(DateTime beginning, DateTime end)
        {
            var result = new TherapySessionSpecification(IsOrientations());
            result.Where(WithinRange(beginning, end));
            return result;
        }

        public static ISpecification<TherapySession> ForTypesWithinDateRange(
            DateTime beginning,
            DateTime end,
            IEnumerable<PrimaryServiceType> typeFilter)
        {
            typeFilter = (typeFilter ?? Enumerable.Empty<PrimaryServiceType>())
                .Distinct()
                .ToArray();
            return new TherapySessionSpecification(WithinRange(beginning, end))
                .Where(session => !typeFilter.Any() || typeFilter.Contains(session.PrimaryService.Type))
                ;
        }

        private static Expression<Func<TherapySession, bool>> IsConsultation()
        {
            return therapySession => therapySession.Room != null;
        }

        private static Expression<Func<TherapySession, bool>> IsDive()
        {
            return therapySession => therapySession.Chamber != null;
        }

        private static Expression<Func<TherapySession, bool>> IsOrientations()
        {
            return therapySession => therapySession.Chamber == null && therapySession.Room == null;
        }

        private static Expression<Func<TherapySession, bool>> WithinRange(DateTime beginning, DateTime end)
        {
            return therapySession =>
                therapySession.DateTime >= beginning &&
                therapySession.DateTime <= end;
        }

        private static Expression<Func<TherapySession, bool>> HasDetails(IBasicSessionDetails details)
        {
            return therapySession =>
                therapySession.PrimaryService.Type == details.PrimaryServiceType &&
                therapySession.Chamber.Id == details.ChamberId &&
                therapySession.Room.Id == details.RoomId &&
                therapySession.Employee.Id == details.EmployeeId;
        }

        private void AddIncludes()
        {
            Include(session => session.PrimaryService.AdditionalServices)
                .Include(session => session.Appointments)
                    .ThenInclude(appointment => appointment.Patient)
                .Include(session => session.Appointments)
                    .ThenInclude(appointment => appointment.Transactions)
                .Include(session => session.Appointments)
                    .ThenInclude(appointment => appointment.AppointmentChamberSeats)
                        .ThenInclude(appointmentChamberSeats => appointmentChamberSeats.ChamberSeat)
                .Include(session => session.Appointments)
                    .ThenInclude(appointment => appointment.AppointmentAdditionalServices)
                        .ThenInclude(join => join.AdditionalService)
                .Include(session => session.Chamber.Seats)
                .Include(session => session.Room)
                .Include(session => session.Employee)
                .Include(session => session.ChamberOperator)
                .Include(session => session.BackupChamberOperator)
                .Include(session => session.InsideAttendant)
                .Include(session => session.BackupInsideAttendant);
        }
    }
}