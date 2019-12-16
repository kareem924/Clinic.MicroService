using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.CQRS;
using Common.General.Entity;
using Security.Core.Entities;
using Security.Core.Repositories;
using Security.Core.Specification;
using Security.Infrastructure.Application.Dto;

namespace Security.Infrastructure.Application.Queries.GetUserPagedResult
{
    public class GetUserPagedResultQueryHandler : IQueryHandler<GetUserPagedResultQuery, PagedResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        public GetUserPagedResultQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<PagedResult<UserDto>> Handle(GetUserPagedResultQuery request, CancellationToken cancellationToken)
        {
            var pagedUserSpecification = new UserSpecification(request, request.Email, request.Name);
            var result = await  _userRepository.GetAllPagedAsync(pagedUserSpecification, request);
            var usersDto = result.Items.Select(MapFromUserDtoToUser);
             return PagedResult<UserDto>.Create(usersDto, request.Page, request.PageSize, result.TotalPages, result.TotalItems);
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
                State = user.Address.State,
                Email = user.Email
            };
            if (user.Roles.Any())
            {
                var roles = user.Roles
                    .Select(userRole => new RoleDto() {Id = userRole.RoleId, Name = userRole.Role.Name})
                    .ToList();
                userDto.Roles = roles;
            }
            return userDto;
        }
    }
}
