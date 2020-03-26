using System.Collections.Generic;
using Portals.Extivita.Core.Payments.Entities;
using Portals.Extivita.SharedKernel.Domain.Entities;

namespace Portals.Extivita.Core.ClinicServices.Entities
{
    public abstract class ServiceBase : FullAuditedEntity
    {
        public decimal Price { get; protected set; }

        public ICollection<PatientCredit> PatientCredits { get; } = new HashSet<PatientCredit>();

        public abstract string GetDisplayName();
    }
}