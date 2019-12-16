using System;
using System.Collections.Generic;
using System.Text;
using Common.CQRS;

namespace Security.Infrastructure.Application.Commands.ActivateUserCommand
{
   public class ActivateUserCommand : ICommand
    {
        public Guid Id { get; set; }

        public bool UserActive { get; set; }
    }
}
