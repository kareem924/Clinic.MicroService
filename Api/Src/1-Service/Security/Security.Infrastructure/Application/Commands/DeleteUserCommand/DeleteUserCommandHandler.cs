using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.Exceptions;
using Common.General.UnitOfWork;
using Microsoft.Extensions.Logging;
using Security.Core.Repositories;

namespace Security.Infrastructure.Application.Commands.DeleteUserCommand
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserCommandHandler(
            IUserRepository userRepository,
            ILogger<DeleteUserCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteUserCommand notification, CancellationToken cancellationToken)
        {
            var deletedUser = await _userRepository.GetByIdAsync(notification.UserId);
            if (deletedUser == null)
            {
                throw new ValidationErrorException("Deleted User Can't be null");
            }
            _userRepository.Delete(deletedUser);
            await _unitOfWork.CommitAsync();

        }
    }
}
