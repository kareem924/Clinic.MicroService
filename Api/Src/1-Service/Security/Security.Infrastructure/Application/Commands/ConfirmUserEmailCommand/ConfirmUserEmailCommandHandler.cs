using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.Exceptions;
using Common.General.UnitOfWork;
using Microsoft.Extensions.Logging;
using Security.Core.Repositories;

namespace Security.Infrastructure.Application.Commands.ConfirmUserEmailCommand
{
    class ConfirmUserEmailCommandHandler : ICommandHandler<ConfirmUserEmailCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ConfirmUserEmailCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ConfirmUserEmailCommandHandler(
            IUserRepository userRepository,
            ILogger<ConfirmUserEmailCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ConfirmUserEmailCommand notification, CancellationToken cancellationToken)
        {
            var updatedUser = await _userRepository.GetByIdAsync(notification.Id);
            if (updatedUser == null)
            {
                throw new ValidationErrorException("Deleted User Can't be null");
            }
            updatedUser.ChangeUserConfirmedEmailStatus(notification.EmailConfirmed);
            await _unitOfWork.CommitAsync();
        }
    }
}
