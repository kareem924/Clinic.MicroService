using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Security.Core.Entities;
using Security.Infrastructure.Helper;

namespace Security.Infrastructure.Application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(UserManager<User> userManager, ILogger<RegisterUserCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task Handle(RegisterUserCommand notification, CancellationToken cancellationToken)
        {
            var user = new User(
                notification.FirstName,
                notification.LastName,
                notification.Email,
                notification.Email, false,
                notification.Address,
                notification.BirthDate,
                notification.PhoneNumber,
                true);
            var result = await _userManager.CreateAsync(user, notification.Password);
            if (!result.Succeeded)
            {
                _logger.LogCritical("user didn't created for this reason " + result.Errors.First());
                throw new Exception(result.Errors.First().Description);
            }
            var roles = new string[] { Roles.NewUser };
            await _userManager.AddToRolesAsync(user, roles);
        }
    }
}
