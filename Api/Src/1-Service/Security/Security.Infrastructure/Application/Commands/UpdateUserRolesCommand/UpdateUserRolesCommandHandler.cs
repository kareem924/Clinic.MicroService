using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.Exceptions;
using Common.General.UnitOfWork;
using Microsoft.Extensions.Logging;
using Security.Core.Entities;
using Security.Core.Repositories;

namespace Security.Infrastructure.Application.Commands.UpdateUserRolesCommand
{
    public class UpdateUserRolesCommandHandler : ICommandHandler<UpdateUserRolesCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserRolesCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserRolesCommandHandler(
            IUserRepository userRepository,
            ILogger<UpdateUserRolesCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserRolesCommand notification, CancellationToken cancellationToken)
        {
            var updatedUser = await _userRepository.GetByIdAsync(notification.Id);
            if (updatedUser == null)
            {
                throw new ValidationErrorException("Deleted User Can't be null");
            }
            var rolesToUpdate = notification.Roles.Select(roleDto => new Role(roleDto.Name)).ToArray();
            updatedUser.UpdateRoles(rolesToUpdate);
            await _unitOfWork.CommitAsync();
        }
    }
}
