using System;
using Portals.Extivita.SharedKernel.Domain.Entities;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class AppointmentChamberSeat : FullAuditedEntity
    {
        public Guid AppointmentId { get; set; }

        public Guid ChamberSeatId { get; set; }

        public Appointment Appointment { get; set; }

        public ChamberSeat ChamberSeat { get; set; }
    }
}
