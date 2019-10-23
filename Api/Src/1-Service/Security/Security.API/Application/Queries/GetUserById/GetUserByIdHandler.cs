using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Security.Core.Entities;
using Security.Core.Repositories;
using Security.Core.Specification;

namespace Security.API.Application.Queries.GetUserById
{
    public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userSpecification = new UserSpecification(request.UserId);
            var user = await _userRepository.FindAsync(userSpecification);
            return user;
        }
    }
}
