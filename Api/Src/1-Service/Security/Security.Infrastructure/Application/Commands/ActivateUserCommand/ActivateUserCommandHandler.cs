using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.Exceptions;
using Common.General.UnitOfWork;
using Microsoft.Extensions.Logging;
using Security.Core.Repositories;

namespace Security.Infrastructure.Application.Commands.ActivateUserCommand
{
    class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ActivateUserCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ActivateUserCommandHandler(
            IUserRepository userRepository,
            ILogger<ActivateUserCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ActivateUserCommand notification, CancellationToken cancellationToken)
        {
            var updatedUser = await _userRepository.GetByIdAsync(notification.Id);
            if (updatedUser == null)
            {
                throw new ValidationErrorException("Deleted User Can't be null");
            }
            updatedUser.ChangeUserActivatedStatus(notification.UserActive);
            await _unitOfWork.CommitAsync();
        }
    }
}
