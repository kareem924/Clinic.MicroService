using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Security.Core.Entities;

namespace Security.Infrastructure.Application.Commands.CreateUserCommand
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly UserManager<User> _userManager;
        public CreateUserCommandValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
            AddRules();
        }
        private void AddRules()
        {
            RuleFor(input => input.Email).NotNull()
                .WithMessage("Email is required.");
            RuleFor(input => input.Email).MustAsync((email, cancellation) => CheckIsValidEmail(email))
                .WithMessage("This Email is already Taken");
            RuleFor(input => input.Password)
                .NotNull().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("MinimumLength Should be more than 6 chars");
        }

        private async Task<bool> CheckIsValidEmail(string email)
        {
            var checkEmail = await _userManager.FindByEmailAsync(email);
            return checkEmail == null;
        }
    }
}
