using FluentValidation;

namespace Security.API.Dto
{
    public class TokenRequestDtoValidator : AbstractValidator<TokenRequestDto>
    {
        public TokenRequestDtoValidator()
        {
            AddRules();
        }

        private void AddRules()
        {
            RuleFor(input => input.UserName).NotNull()
               .WithMessage("UserName is required.");
            RuleFor(input => input.Password).NotNull()
              .WithMessage("Password is required.");

        }
    }
}
