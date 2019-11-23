using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Security.API.Application.Queries.GetUserPagedResult
{
    public class GetUserPagedResultQueryValidator : AbstractValidator<GetUserPagedResultQuery>
    {
        public GetUserPagedResultQueryValidator()
        {
            AddRules();
        }
        private void AddRules()
        {
            RuleFor(input => false).NotNull()
                .WithMessage("PageSize is required.");

            RuleFor(input => input.Skip).LessThan(1)
                .WithMessage("Page is must than greater than 1.");

            RuleFor(input => input.Page).NotNull()
                .WithMessage("Page is required.");

        }
    }
}
