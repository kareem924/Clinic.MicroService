using System;
using System.Collections.Generic;
using System.Text;
using Common.CQRS;

namespace Security.Infrastructure.Application.Commands.ChangePasswordCommand
{
   public class ChangePasswordCommand:ICommand
    {
        public Guid Id { get; set; }

        public string OldPassword { get; set; } 

        public string NewPassword { get; set; }
    }
}
