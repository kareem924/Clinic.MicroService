using System.Collections.Generic;

namespace Portals.Extivita.Core.ClinicServices.Entities
{
    /// <summary>
    /// Service that clinic offer to patients
    /// </summary>
    public class PrimaryService : ServiceBase
    {
        public PrimaryServiceType Type { get; private set; }

        public ICollection<AdditionalService> AdditionalServices { get; } = new HashSet<AdditionalService>();

        public PrimaryService(PrimaryServiceType type, decimal price)
        {
            Type = type;
            Price = price;
        }

        public override string ToString()
        {
            return $"PrimaryService{{{Type}}}";
        }

        public override string GetDisplayName() => Type.ToString();
    }
}