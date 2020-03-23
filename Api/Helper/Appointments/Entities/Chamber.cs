using System.Collections.Generic;
using Portals.Extivita.SharedKernel.Domain.Entities;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class Chamber : FullAuditedEntity
    {
        public string Name { get; set; }

        public string AssignedColor { get; set; } //=> "#26b1e4";

        public ICollection<ChamberSeat> Seats { get; set; } = new HashSet<ChamberSeat>();

        public string ActualMapPath { get; set; }

        //public ICollection<TimeSlot> TimeSlots { get; set; } = new HashSet<TimeSlot>();
    }
}
