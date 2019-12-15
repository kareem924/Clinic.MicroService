using System;
using Common.CQRS;
using FluentValidation;

namespace Security.Infrastructure.Application.Commands.DeleteUserCommand
{
    public class DeleteUserCommand : ICommand
    {
        public Guid UserId { get; set; }
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(input => input.UserId).NotNull()
                .WithMessage("UserId required.");
        }
    }
}
