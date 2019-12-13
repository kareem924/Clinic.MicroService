using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.UnitOfWork;
using Security.Core.Repositories;
using Security.Core.Specification;

namespace Security.Infrastructure.Application.Commands.UpdateUserRefreshToken
{
    public class UpdateUserRefreshTokenCommandHandler : ICommandHandler<UpdateUserRefreshTokenCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserRefreshTokenCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserRefreshTokenCommand notification, CancellationToken cancellationToken)
        {
            var userSpecification = new UserSpecification(notification.UserId);
            var user = await _userRepository.FindAsync(userSpecification);
            user.AddRefreshToken(notification.RefreshToken, notification.UserId, notification.RemoteIpAddress);
            await _userRepository.UpdateAsync(user, notification.UserId);
            await _unitOfWork.CommitAsync();
        }
    }
}
