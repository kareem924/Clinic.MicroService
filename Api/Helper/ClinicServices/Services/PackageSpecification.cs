using System;
using Portals.Extivita.Core.ClinicServices.Entities;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification;

namespace Portals.Extivita.Core.ClinicServices.Services
{
    public class PackageSpecification : Specification<Package>
    {
        private PackageSpecification()
        {
            AddIncludes();
        }

        public static PackageSpecification Empty()
        {
            return new PackageSpecification();
        }

        public static PackageSpecification ById(Guid id)
        {
            var result = new PackageSpecification();
            result.Where(package => package.Id == id);
            return result;
        }

        private void AddIncludes()
        {
            this
                .Include(package => package.PackageItems)
                .ThenInclude(packageItem => packageItem.Service);
        }
    }
}