using System;
using Common.CQRS;
using FluentValidation;

namespace Security.Infrastructure.Application.Commands.DeleteUserCommand
{
    public class DeleteUserCommand : ICommand
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(input => input.Id).NotNull()
                .WithMessage("UserId required.");
        }
    }
}
