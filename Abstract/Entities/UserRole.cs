using System;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
