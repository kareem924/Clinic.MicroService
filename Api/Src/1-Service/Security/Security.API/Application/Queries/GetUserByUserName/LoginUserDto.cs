using System;
using System.Collections.Generic;

namespace Security.API.Application.Queries.GetUserByUserName
{
    public class LoginUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UpdateBy { get; set; }

        public DateTime BirthDate { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }

        public IEnumerable<RefreshTokenDto> RefreshTokens { get; set; }
    }

    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class RefreshTokenDto
    {
        public Guid Id { get; set; }

        public string Token { get; set; }
        public bool Active { get; set; }
        public string RemoteIpAddress { get; set; }
    }
}
