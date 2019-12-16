using System;
using Common.CQRS;

namespace Security.Infrastructure.Application.Commands.ConfirmUserEmailCommand
{
    public class ConfirmUserEmailCommand : ICommand
    {
        public Guid Id { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
