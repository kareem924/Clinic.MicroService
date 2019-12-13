using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using MassTransit.Initializers;
using Security.Core.Entities;
using Security.Core.Repositories;
using Security.Core.Specification;
using Security.Infrastructure.Application.Dto;

namespace Security.Infrastructure.Application.Queries.GetUserDtoId
{
    public class GetUserDtoByIdQueryHandler : IQueryHandler<GetUserDtoByIdQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        public GetUserDtoByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDto> Handle(GetUserDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var userSpecification = new UserSpecification(request.UserId);
            var userDto = await _userRepository.FindAsync(userSpecification).Select(MapFromUserDtoToUser);
            return userDto;
        }
        private UserDto MapFromUserDtoToUser(User user)
        {
            var userDto = new UserDto()
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsDeleted = user.IsDeleted,
                CreatedBy = user.CreatedBy,
                CreationDate = user.CreationDate,
                UpdateBy = user.UpdateBy,
                City = user.Address.City,
                Country = user.Address.Country,
                Street = user.Address.Street,
                State = user.Address.State
            };
            if (user.Roles.Any())
            {
                var roles = user.Roles
                    .Select(userRole => new RoleDto() { Id = userRole.RoleId, Name = userRole.Role.Name })
                    .ToList();
                userDto.Roles = roles;
            }
            return userDto;
        }
    }
}
