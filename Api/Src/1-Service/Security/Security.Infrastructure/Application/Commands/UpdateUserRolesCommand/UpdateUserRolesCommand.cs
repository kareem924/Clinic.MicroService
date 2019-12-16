using System;
using System.Collections.Generic;
using System.Text;
using Common.CQRS;
using Security.Infrastructure.Application.Dto;

namespace Security.Infrastructure.Application.Commands.UpdateUserRolesCommand
{
    public class UpdateUserRolesCommand:ICommand
    {
        public Guid Id { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }
    }
}
