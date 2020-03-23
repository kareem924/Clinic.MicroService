using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification;
using System;

namespace Portals.Extivita.Core.ClinicServices.Services
{
    public class PrimaryServiceSpecification : Specification<PrimaryService>
    {
        private PrimaryServiceSpecification()
        {
            AddIncludes();
        }

        public static ISpecification<PrimaryService> ForAll()
        {
            return new PrimaryServiceSpecification();
        }

        public static ISpecification<PrimaryService> ForId(Guid id)
        {
            return new PrimaryServiceSpecification()
                .Where(service => service.Id == id);
        }

        public static ISpecification<PrimaryService> ByType(PrimaryServiceType type)
        {
            return new PrimaryServiceSpecification()
                .Where(service => service.Type == type);
        }

        private void AddIncludes()
        {
            Include(service => service.AdditionalServices);
        }
    }
}