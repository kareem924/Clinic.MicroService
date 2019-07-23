using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Entities
{

    public class Role : IdentityRole<Guid>
    {
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

    }
}
