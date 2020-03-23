using System.Collections.Generic;
using System.Linq;
using Portals.Extivita.SharedKernel.Domain.Entities;

namespace Portals.Extivita.Core.ClinicServices.Entities
{
    /// <summary>
    /// The package that describe the offer and the price
    /// </summary>
    public class Package : FullAuditedEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public ICollection<PackageItem> PackageItems { get; private set; } = new HashSet<PackageItem>();

        public decimal GetActualPrice()
        {
            return PackageItems.Sum(item =>item.Service.Price * item.Count);
        }
    }
}