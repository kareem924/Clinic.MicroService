using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.CQRS;
using Security.API.Application.Queries.GetUserPagedResult;

namespace Security.API.Application.Commands.CreateUserCommand
{
    public class CreateUserCommand : ICommand
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }

    }

}
