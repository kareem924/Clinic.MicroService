using System;
using Portals.Extivita.SharedKernel.Domain.ValueObject;
using Portals.Extivita.SharedKernel.Timing;

namespace Portals.Extivita.Core.Appointments.Entities
{
    [ValueObject]
    public class HoldInfo
    {
        public string HeldFor { get; set; }

        public DateTime? HeldTo { get; set; }

        public bool OnHold => !string.IsNullOrEmpty(HeldFor) && HeldTo.HasValue && HeldTo > Clock.Now;

        public void Release()
        {
            HeldFor = null;
            HeldTo = null;
        }
    }
}
