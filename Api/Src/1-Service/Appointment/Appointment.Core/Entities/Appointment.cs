using Appointment.Core.Enums;
using Common.General.Entity;
using Common.General.Interfaces;
using System;


namespace Appointment.Core.Entities
{

    public class Appointment : GuidIdEntity, IAggregateRoot
    {
        public AppointmentStatus Status { get; private set; } = AppointmentStatus.NotArrived;

        public string CancelReason { get; private set; }

        public TimeSpan? ArrivalTime { get; private set; }

        public int Version { get; private set; } = 1;


        public Session Session { get; private set; }

        public Guid PatientId { get; protected set; }

        public int ReferenceNumber { get; protected set; }


        private Appointment()
        {
        }

        public Appointment(Guid patientId)
        {
            if (patientId == null || patientId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(patientId));
            }
            PatientId = patientId;
            //AddDomainEvent(new AppointmentCreatedEvent(Id.ToString()));
        }

        public void SetStatus(AppointmentStatus status)
        {
            ValidateNotCanceled();
            Status = status;
            if (Status == AppointmentStatus.CheckedIn)
            {
                ArrivalTime = DateTime.Now.TimeOfDay;
            }
        }

        public void Cancel(string reason, bool refundPayment)
        {
            ValidateNotCanceled();
            Status = AppointmentStatus.Canceled;
            CancelReason = reason;
            //AddDomainEvent(new AppointmentCanceledEvent(Id, refundPayment));
        }



        //public void SetAdditionalServices(IEnumerable<AdditionalService> newAdditionalServices)
        //{
        //    var existingAdditionalServices = AppointmentAdditionalServices
        //        .Select(service => service.AdditionalService)
        //        .ToArray();
        //    var removedAppointmentServices = existingAdditionalServices.Except(newAdditionalServices)
        //        .Select(additionalService =>
        //            AppointmentAdditionalServices.Single(join => join.AdditionalService.Id == additionalService.Id))
        //        .ToArray();
        //    var addedServices = newAdditionalServices.Except(existingAdditionalServices)
        //        .Select(additionalService => new AppointmentAdditionalService(additionalService, this))
        //        .ToArray();
        //    AppointmentAdditionalServices.RemoveRange(removedAppointmentServices);
        //    AppointmentAdditionalServices.AddRange(addedServices);
        //}



        //public override IReadOnlyCollection<ServiceBase> GetServices()
        //{
        //    var primaryService = Session.PrimaryService;
        //    var additionalServices = AppointmentAdditionalServices
        //        .Select(join => join.AdditionalService)
        //        .Concat(new ServiceBase[] { primaryService })
        //        .ToArray();
        //    return additionalServices;
        //}

        public void Reschedule(DateTime dateTime)
        {
            Version++;
            //AddDomainEvent(new AppointmentRescheduledEvent(Id, dateTime));
        }

        private void ValidateNotCanceled()
        {
            if (Status == AppointmentStatus.Canceled)
            {
                throw new InvalidOperationException("Appointment is canceled.");
            }
        }


    }
}
