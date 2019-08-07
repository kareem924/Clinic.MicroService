using System;
using System.Collections.Generic;
using System.Text;
using Common.General.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Security.Core.Entities
{
    public class Role : IdentityRole<Guid>, IAggregateRoot
    {
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

        public Role()
        {
        }

        public Role(string name) : base(name)
        {
        }
    }
}
