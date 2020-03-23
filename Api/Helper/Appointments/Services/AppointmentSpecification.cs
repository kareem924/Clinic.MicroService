using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.Core.Appointments.Enums;
using Portals.Extivita.Core.Payments.Entities;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification;
using Portals.Extivita.SharedKernel.Extensions;
using Portals.Extivita.SharedKernel.Helpers;
using Portals.Extivita.SharedKernel.Security;
using Portals.Extivita.SharedKernel.Timing;

namespace Portals.Extivita.Core.Appointments.Services
{
    public sealed class AppointmentSpecification : Specification<Appointment>
    {
        private AppointmentSpecification()
        {
            AddIncludes();
        }

        public static AppointmentSpecification ByIds(IEnumerable<Guid> ids)
        {
            var result = new AppointmentSpecification();
            result.Where(appointment => ids.Contains(appointment.Id));
            return result;
        }

        public static AppointmentSpecification ById(Guid id)
        {
            var result = new AppointmentSpecification();
            result.Where(appointment => appointment.Id == id);
            return result;
        }

        public static AppointmentSpecification ByPatientOrRelative(Guid patientId)
        {
            var result = new AppointmentSpecification();
            result.Where(HasPatientOrRelative(patientId));
            return result;
        }

        public static AppointmentSpecification ForPatientCreditPaid(Guid patientId)
        {
            var result = ByPatientOrRelative(patientId);
            result.Where(IsPaid(PaymentType.PatientCredit));
            return result;
        }

        public static AppointmentSpecification ForPatientCreditPaidServiceAndPatient(Guid patientId, Guid serviceId)
        {
            var result = ByPatientOrRelative(patientId);
            result
                .Where(appointment =>
                    appointment.TherapySession.PrimaryService.Id == serviceId ||
                    appointment.AppointmentAdditionalServices
                        .Any(join => join.AdditionalServiceId == serviceId))                
                .Where(IsPaid(PaymentType.PatientCredit));
            return result;
        }

        public static AppointmentSpecification ByTherapySessionAndPatient(Guid therapySessionId, Guid patientId)
        {
            var result = new AppointmentSpecification();
            result.Where(appointment =>
                appointment.TherapySession.Id == therapySessionId &&
                appointment.Patient.Id == patientId);
            return result;
        }

        public static AppointmentSpecification BySeats(IEnumerable<Guid> seatIds)
        {
            var result = new AppointmentSpecification();
            result.Where(appointment =>
                appointment.AppointmentChamberSeats
                    .Any(chamberSeat => seatIds.Contains(chamberSeat.ChamberSeat.Id)));
            return result;
        }

        public static AppointmentSpecification ByDay(DateTime day)
        {
            var result = new AppointmentSpecification();
            result.Where(appointment => appointment.TherapySession.DateTime.Date == day);
            return result;
        }

        public static AppointmentSpecification ForTodayDivesWithoutPayment()
        {
            var result = new AppointmentSpecification();
            result
                .Where(IsDive())
                .Where(OccursToday())
                .Where(HasNoPayment());
            return result;
        }

        public static AppointmentSpecification ForPatientsDivesWithinDateRange(
          (Guid Id,
          DateTime beginDay,
          DateTime endDay)[] patientAndRanges)
        {
            Check.NotEmpty(patientAndRanges, nameof(patientAndRanges));
            var result = new AppointmentSpecification();
            var predicate = patientAndRanges.Select(patientAndRange =>
                    HasPatientOrRelative(patientAndRange.Id).And(
                        WithinDayRange(patientAndRange.beginDay, patientAndRange.endDay)))
                .Aggregate((previous, next) => previous.Or(next));
            result
                .Where(IsDive())
                .Where(predicate);
            return result;
        }

        private static Expression<Func<Appointment, bool>> WithinDayRange(DateTime beginDay, DateTime endDay)
        {
            return appointment =>
                appointment.TherapySession.DateTime.Date >= beginDay.Date &&
                appointment.TherapySession.DateTime.Date < endDay.Date;
        }

        public static AppointmentSpecification ForPatientAtTime(
            Guid patientId,
            DateTime dateTime)
        {

            var result = new AppointmentSpecification();
            result.Where(appointment =>
                appointment.Patient.Id == patientId &&
                appointment.TherapySession.DateTime == dateTime);
            return result;
        }

        private static Expression<Func<Appointment, bool>> IsPaid(PaymentType? paymentType)
        {
            return appointment =>
                appointment.Transactions.Any() &&
                appointment.Transactions
                    .OrderByDescending(transaction => transaction.CreationDate)
                    .Take(1)
                    .Any(transaction =>
                        transaction.Status == TransactionStatus.Completed &&
                        (!paymentType.HasValue || transaction.PaymentMethod.Type == paymentType));
        }

        private static Expression<Func<Appointment, bool>> HasPatientOrRelative(Guid patientId)
        {
            return appointment =>
                appointment.Patient.Id == patientId ||
                appointment.Patient.Relative.Id == patientId;
        }

        private static Expression<Func<Appointment, bool>> IsDive()
        {
            return appointment => appointment.TherapySession.Chamber != null;
        }

        private static Expression<Func<Appointment, bool>> OccursToday()
        {
            var today = Clock.Today;
            return appointment => appointment.TherapySession.DateTime.Date == today;
        }

        private static Expression<Func<Appointment, bool>> HasNoPayment()
        {
            return appointment => !appointment.Transactions.Any();
        }

        public AppointmentSpecification NotCanceled()
        {
            Where(appointment => appointment.Status != AppointmentStatus.Canceled);
            return this;
        }

        private void AddIncludes()
        {
            Include(appointment => appointment.AppointmentAdditionalServices)
                    .ThenInclude(appointmentService => appointmentService.AdditionalService)
                .Include(appointment => appointment.TherapySession.PrimaryService.AdditionalServices)
                .Include(appointment => appointment.TherapySession.Chamber)
                .Include(appointment => appointment.TherapySession.Room)
                .Include(appointment => appointment.TherapySession.Employee)
                .Include(appointment => appointment.Patient.Relative)
                .Include(appointment => appointment.Transactions)
                    .ThenInclude(transaction => transaction.PaymentMethod)
                .Include(appointment => appointment.AppointmentChamberSeats)
                    .ThenInclude(appointmentChamberSeats => appointmentChamberSeats.ChamberSeat);
        }
    }
}
