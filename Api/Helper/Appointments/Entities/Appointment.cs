using System;
using System.Collections.Generic;
using System.Linq;
using Portals.Extivita.Core.Appointments.Entities.PatientChecklists;
using Portals.Extivita.Core.Appointments.Enums;
using Portals.Extivita.Core.Appointments.Events;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.Core.Payments.Entities;
using Portals.Extivita.SharedKernel.Extensions;
using Portals.Extivita.SharedKernel.Helpers;
using Portals.Extivita.SharedKernel.Timing;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class Appointment : OrderedItemBase
    {
        public AppointmentStatus Status { get; private set; } = AppointmentStatus.NotArrived;

        public string CancelReason { get; private set; }

        public TimeSpan? ArrivalTime { get; private set; }

        public int Version { get; private set; } = 1;

        public ConsultationNoteChecklist ConsultationNoteChecklist { get; private set; }

        public PatientDiveNoteChecklist PatientDiveNoteChecklist { get; private set; }

        public PostDiveChecklist PostDiveChecklist { get; private set; }

        public PreDivePatientChecklist PreDiveChecklist { get; private set; }

        public SoapNoteChecklist SoapNoteChecklist { get; private set; }

        public TriageDiveNoteChecklist TriageDiveNoteChecklist { get; private set; }

        public TherapySession TherapySession { get; private set; }

        //TODO: (asafan) use readonly collection
        public ICollection<AppointmentChamberSeat> AppointmentChamberSeats { get; }
            = new HashSet<AppointmentChamberSeat>();

        public ICollection<AppointmentAdditionalService> AppointmentAdditionalServices { get; }
            = new HashSet<AppointmentAdditionalService>();

        public IReadOnlyCollection<ChamberSeat> Seats => AppointmentChamberSeats
            .Select(appointmentChamberSeat => appointmentChamberSeat.ChamberSeat)
            .ToArray();

        public IReadOnlyCollection<AdditionalService> AdditionalServices =>
            AppointmentAdditionalServices.Select(join => join.AdditionalService).ToArray();

        private Appointment()
        {
        }

        public Appointment(User patient)
        {
            Patient = patient ?? throw new ArgumentNullException(nameof(patient));
            //AddDomainEvent(new AppointmentCreatedEvent(Id.ToString()));
        }
       
        public void SetStatus(AppointmentStatus status)
        {
            ValidateNotCanceled();
            var isDive = TherapySession.PrimaryService.Type == PrimaryServiceType.Dive;
            if (isDive && Status == AppointmentStatus.InConsult )
            {
               throw new InvalidOperationException("In Consult is not valid status for Dive.");
            }
            if (isDive && Transaction?.Status != TransactionStatus.Completed)
            {
                throw new InvalidOperationException("Can not update appointment without completed payment.");
            }
            Status = status;
            if (Status == AppointmentStatus.CheckedIn)
            {
                ArrivalTime = Clock.Now.TimeOfDay;
            }
        }

        public void Cancel(string reason, bool refundPayment)
        {
            ValidateNotCanceled();
            Status = AppointmentStatus.Canceled;
            CancelReason = reason;
            AddDomainEvent(new AppointmentCanceledEvent(Id, refundPayment));
        }

        public void FillChecklist(ConsultationNoteChecklist checklist)
        {
            ConsultationNoteChecklist = checklist;
        }

        public void FillChecklist(PatientDiveNoteChecklist checklist)
        {
            PatientDiveNoteChecklist = checklist;
            Status = PatientDiveNoteChecklist.RequireDoctorApproval
                ? AppointmentStatus.MedicalHistoryChange
                : AppointmentStatus.PreDiveVital;
        }

        public void FillChecklist(PostDiveChecklist checklist)
        {
            PostDiveChecklist = checklist;
            if (PostDiveChecklist.NeedsReview && TriageDiveNoteChecklist == null)
            {
                var triageChecklist = TriageDiveNoteChecklist.FromPostDiveChecklist(PatientDiveNoteChecklist);
                FillChecklist(triageChecklist);
            }
        }

        public void FillChecklist(PreDivePatientChecklist checklist)
        {
            PreDiveChecklist = checklist;
        }

        public void FillChecklist(SoapNoteChecklist checklist)
        {
            SoapNoteChecklist = checklist;
        }

        public void FillChecklist(TriageDiveNoteChecklist checklist)
        {
            TriageDiveNoteChecklist = checklist;
            if (TriageDiveNoteChecklist.RequireDoctorApproval)
            {
                Status = AppointmentStatus.PendingPostDive;
            }
        }

        public void SetAdditionalServices(IEnumerable<AdditionalService> newAdditionalServices)
        {
            var existingAdditionalServices = AppointmentAdditionalServices
                .Select(service => service.AdditionalService)
                .ToArray();
            var removedAppointmentServices = existingAdditionalServices.Except(newAdditionalServices)
                .Select(additionalService =>
                    AppointmentAdditionalServices.Single(join => join.AdditionalService.Id == additionalService.Id))
                .ToArray();
            var addedServices = newAdditionalServices.Except(existingAdditionalServices)
                .Select(additionalService => new AppointmentAdditionalService(additionalService, this))
                .ToArray();
            AppointmentAdditionalServices.RemoveRange(removedAppointmentServices);
            AppointmentAdditionalServices.AddRange(addedServices);
        }

        public void SetSeats(IEnumerable<ChamberSeat> seats)
        {
            Check.NotNull(seats, nameof(seats));
            var appointmentSeats = seats
                .Select(seat => new AppointmentChamberSeat { ChamberSeat = seat }).ToArray();
            //TODO: (asafan) handle what to be deleted and what to be added
            AppointmentChamberSeats.Clear();
            AppointmentChamberSeats.AddRange(appointmentSeats);
         
        }

        public override IReadOnlyCollection<ServiceBase> GetServices()
        {
            var primaryService = TherapySession.PrimaryService;
            var additionalServices = AppointmentAdditionalServices
                .Select(join => join.AdditionalService)
                .Concat(new ServiceBase[] { primaryService })
                .ToArray();
            return additionalServices;
        }

        public void Reschedule(DateTime dateTime)
        {
            Version++;
            AddDomainEvent(new AppointmentRescheduledEvent(Id, dateTime));
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
