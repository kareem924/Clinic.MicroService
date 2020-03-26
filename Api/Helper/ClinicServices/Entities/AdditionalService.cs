using System;
using System.Collections.Generic;
using Portals.Extivita.Core.Appointments.Entities;

namespace Portals.Extivita.Core.ClinicServices.Entities
{

    /// <summary>
    /// Additional service offered to patient in an appointment
    /// </summary>
    public class AdditionalService : ServiceBase
    {
        public string Name { get; private set; }

        public Guid PrimaryServiceId { get; set; }

        public PrimaryService PrimaryService { get; set; }

        public ICollection<AppointmentAdditionalService> AppointmentAdditionalServices { get; }
            = new HashSet<AppointmentAdditionalService>();

        public AdditionalService(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public AdditionalService()
        {
        }

        public void Update(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"AdditionalService{{{Name}}}";
        }

        public override string GetDisplayName() => Name;
    }
}