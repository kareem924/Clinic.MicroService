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

        public string FullName => $"{FirstName}  {LastName}";

        private readonly List<UserRole> _roles = new List<UserRole>();

        public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();


        public User() { }

        public User(
            string firstName,
            string lastName,
            string userName,
            string email,
            bool emailConfirmed,
            Address address,
            DateTime birthDate,
            string phoneNumber)
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
            Address = address;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
        }

        public void AddRole(Role role)
        {
            _roles.Add(new UserRole(){Role = role ,UserId = Id });
        }

        public bool HasRole(Role role)
        {
            return _roles.Any(rt => rt.Role== role);
        }

        public bool HasValidRefreshToken(string refreshToken)
        {
            return _refreshTokens.Any(userRefreshToken =>
                userRefreshToken.Token == refreshToken &&
                userRefreshToken.Active);
        }

        public void AddRefreshToken(string token, Guid userId, string remoteIpAddress, double daysToExpire = 5)
        {
            _refreshTokens.Add(new RefreshToken(
                token,
                DateTime.UtcNow.AddDays(daysToExpire), userId, remoteIpAddress));
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
        }
    }
}
