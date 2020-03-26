using System;
using Portals.Extivita.SharedKernel.Domain.Entities;

namespace Portals.Extivita.Core.ClinicServices.Entities
{
    public class PackageItem : FullAuditedEntity
    {
        public int Count { get; set; }

        public Guid ServiceId { get; set; }

        public ServiceBase Service { get; set; }
    }
}