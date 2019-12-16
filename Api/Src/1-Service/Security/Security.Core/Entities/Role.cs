using System;
using System.Collections.Generic;
using Common.General.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Security.Core.Entities
{
    public class Role : IdentityRole<Guid>, IAggregateRoot,IEntity
    {
        private readonly List<UserRole> _users = new List<UserRole>();

        public IReadOnlyCollection<UserRole> Users => _users.AsReadOnly();

        public Role()
        {
        }

        public Role(string name) : base(name)
        {
        }
    }
}
