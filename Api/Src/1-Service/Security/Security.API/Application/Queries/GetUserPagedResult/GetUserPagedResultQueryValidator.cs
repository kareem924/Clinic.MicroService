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
            RuleFor(input => input).NotNull().WithMessage("The Query is required");

            RuleFor(input => input.PageSize).GreaterThan(0)
                .WithMessage("PageSize is required.");

            RuleFor(input => input.Page).GreaterThan(0)
                .WithMessage("Page is required.");

        }
    }
}
