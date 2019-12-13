using System;
using System.Collections.Generic;

namespace Security.Infrastructure.Application.Dto
{
    public class UserDto
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UpdateBy { get; set; }

        public DateTime BirthDate { get; set; }

        public string Street { get;  set; }

        public string City { get;  set; }

        public string State { get;  set; }

        public string Country { get;  set; }

        public IEnumerable<RoleDto> Roles { get; set; }

    }
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
