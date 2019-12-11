using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Security.API.Application.Commands.RegisterUser;
using Security.Core.Entities;
using Security.Infrastructure.Helper;

namespace Security.API.Application.Commands.CreateUserCommand
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public CreateUserCommandHandler(UserManager<User> userManager, ILogger<RegisterUserCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task Handle(CreateUserCommand notification, CancellationToken cancellationToken)
        {
            var user = new User(
                notification.FirstName,
                notification.LastName,
                notification.Email,
                notification.Email, false,
                null,
                notification.BirthDate,
                notification.PhoneNumber,
                notification.IsActive);
            var result = await _userManager.CreateAsync(user, notification.Password);
            if (!result.Succeeded)
            {
                _logger.LogCritical("user didn't created for this reason " + result.Errors.First());
                throw new Exception(result.Errors.First().Description);
            }
            await _userManager.AddToRolesAsync(user, notification.Roles.Select(role => role.Name));
        }

    }
}
