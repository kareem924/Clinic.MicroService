using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.Exceptions;
using Common.General.UnitOfWork;
using Microsoft.Extensions.Logging;
using Security.Core.Entities;
using Security.Core.Repositories;
using Security.Infrastructure.Application.Commands.DeleteUserCommand;

namespace Security.Infrastructure.Application.Commands.UpdateUserCommand
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserCommandHandler(IUserRepository userRepository, ILogger<UpdateUserCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserCommand notification, CancellationToken cancellationToken)
        {
            var updatedUser = await _userRepository.GetByIdAsync(notification.Id);
            if (updatedUser == null)
            {
                throw new ValidationErrorException("updatedUser User Can't be null");
            }
            var updatedRoles = notification.Roles.Select(roleDto => new Role(roleDto.Name)).ToArray();
            updatedUser.UpdateUserData(
                notification.FirstName,
                notification.LastName,
                notification.Email,
                notification.Email,
                notification.EmailConfirmed,
                null,
                notification.BirthDate,
                notification.PhoneNumber,
                notification.IsActive);
            updatedUser.UpdateRoles(updatedRoles);
            await _userRepository.UpdateAsync(updatedUser, notification.Id);
            await _unitOfWork.CommitAsync();
        }
    }
}
