using System;
using System.Collections.Generic;
using System.Linq;
using Common.General.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace Security.Core.Entities
{
    public sealed class User : IdentityUser<Guid>,IAggregateRoot,IEntity
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public bool IsDeleted { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UpdateBy { get; set; }

        public DateTime UpdatingDate { get; set; }

     

        private readonly List<UserRole> _roles = new List<UserRole>();

        public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

        public User() { }

        public User(
            string firstName,
            string lastName,
            string userName,
            string email)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
        }

       

        public bool HasRole(Role role)
        {
            return _roles.Any(rt => rt.Role == role);
        }

      
    }
}
