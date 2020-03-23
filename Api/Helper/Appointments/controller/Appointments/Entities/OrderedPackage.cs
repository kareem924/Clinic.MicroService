using System;
using System.Collections.Generic;
using System.Linq;
using Portals.Extivita.Core.Authentication.Entities;
using Portals.Extivita.Core.ClinicServices.Entities;

namespace Portals.Extivita.Core.Appointments.Entities
{
    public class OrderedPackage : OrderedItemBase
    {
        public Package Package { get; private set; }

        private OrderedPackage()
        {
        }

        public OrderedPackage(Package package, User patient)
        {
            Package = package;
            Patient = patient;
        }

        public override IReadOnlyCollection<ServiceBase> GetServices()
        {
            return Package.PackageItems
                .SelectMany(item => Enumerable.Repeat(item.Service, item.Count))
                .ToArray();
        }
    }
}