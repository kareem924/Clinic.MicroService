using System;
using Portals.Extivita.SharedKernel.Domain.Entities;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class ChamberSeat : FullAuditedEntity
    {
        public string Name { get; set; }

        public Guid ChamberId { get; set; }

        public Chamber Chamber { get; set; }

        public void Update(string name)
        {
            Name = name;
        }
        //public ICollection<AppointmentChamberSeat> AppointmentChamberSeats { get; set; }
        //    = new HashSet<AppointmentChamberSeat>();
    }
}
