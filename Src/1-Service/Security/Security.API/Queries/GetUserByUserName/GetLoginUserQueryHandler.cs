using Common.CQRS;
using Microsoft.AspNetCore.Identity;
using Security.Core.Entities;
using Security.Core.Repositories;
using Security.Core.Specification;
using System.Threading;
using System.Threading.Tasks;
using Common.General.Repository;

namespace Security.API.Queries.GetUserByUserName
{
    public class GetLoginUserQueryHandler : IQueryHandler<GetLoginUserQuery, LoginUserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapperService _mapperService;
        public GetLoginUserQueryHandler(
            UserManager<User> userManager,
            IUserRepository userRepository,
            IMapperService mapperService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mapperService = mapperService;
        }
        public async Task<LoginUserDto> Handle(GetLoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return null;
            }
            var successPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!successPassword)
            {
                return null;
            }
            var userSpecification = new UserSpecification(user.Id);
            var loggedUser = await _userRepository.FindAsync(userSpecification);
            return _mapperService.MapUserToLoginUserDto(loggedUser);
        }
    }
}
