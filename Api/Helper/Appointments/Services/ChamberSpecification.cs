using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Portals.Extivita.Core.Appointments.Entities;
using Portals.Extivita.SharedKernel.Domain.Repositories.Specification;
using Portals.Extivita.SharedKernel.Timing;

namespace Portals.Extivita.Core.Appointments.Services
{
    public sealed class ChamberSpecification : Specification<Chamber>
    {
        private ChamberSpecification(Expression<Func<Chamber, bool>> criteria) : base(criteria)
        {
            AddIncludes();
        }

        public static ChamberSpecification ById(Guid id)
        {
            return new ChamberSpecification(chamber => chamber.Id == id);
        }

        private void AddIncludes()
        {
            Include(chamber => chamber.Seats);
        }
    }
}
