using System;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.SharedKernel.Domain.Entities;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class AppointmentAdditionalService : Audited
    {
        public Guid AppointmentId { get; private set; }

        public Guid AdditionalServiceId { get; private set; }

        public Appointment Appointment { get; private set; }

        public AdditionalService AdditionalService { get; private set; }

        private AppointmentAdditionalService()
        {
        }

        public AppointmentAdditionalService(AdditionalService additionalService, Appointment appointment)
        {
            AdditionalService = additionalService;
            Appointment = appointment;
        }
    }
}