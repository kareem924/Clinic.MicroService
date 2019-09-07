using System;
using System.Collections.Generic;
using System.Linq;
using Common.General.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace Security.Core.Entities
{
    public sealed class User : IdentityUser<Guid>, IAggregateRoot, IEntity
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public bool IsDeleted { get; set; }

        public Guid CreatedBy { get; private set; }

        public DateTime CreationDate { get; private set; }

        public Guid UpdateBy { get; private set; }

        public DateTime BirthDate { get; private set; }

        public Address Address { get; private set; }


        private readonly List<UserRole> _roles = new List<UserRole>();

        public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

        public User() { }

        public User(
            string firstName,
            string lastName,
            string userName,
            string email,
            bool emailConfirmed)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            EmailConfirmed = emailConfirmed;
        }

        public User(
            string firstName,
            string lastName,
            string userName,
            string email,
            Address address,
            DateTime birthDate)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            Address = address;
            BirthDate = birthDate;
        }

        public bool HasRole(Role role)
        {
            return _roles.Any(rt => rt.Role == role);
        }


    }
}
