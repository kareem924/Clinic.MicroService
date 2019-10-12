using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.UnitOfWork;
using Security.Core.Repositories;
using Security.Core.Specification;

namespace Security.API.Commands.ExchangeRefreshToken
{
    public class ExchangeRefreshTokenCommandHandler : ICommandHandler<ExchangeRefreshTokenCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ExchangeRefreshTokenCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ExchangeRefreshTokenCommand notification, CancellationToken cancellationToken)
        {
            var userSpecification = new UserSpecification(notification.UserId);
            var user = await _userRepository.FindAsync(userSpecification);
            user.RemoveRefreshToken(notification.OldRefreshToken);
            user.AddRefreshToken(notification.NewRefreshToken, user.Id, "");
            await _userRepository.UpdateAsync(user, user.Id);
            await _unitOfWork.CommitAsync();
        }
    }
}
