using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.Exceptions;
using Microsoft.AspNetCore.Identity;
using Security.Core.Entities;

namespace Security.Infrastructure.Application.Commands.ChangePasswordCommand
{
    class ChangePasswordCommandHandler:ICommandHandler<ChangePasswordCommand>

    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ChangePasswordCommand notification, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(notification.Id.ToString());
            var result = await _userManager.ChangePasswordAsync(user, notification.OldPassword, notification.NewPassword);
            if (!result.Succeeded)
            {
                throw new ValidationErrorException(result.Errors?.FirstOrDefault()?.Description);
            }
        }
    }
}
