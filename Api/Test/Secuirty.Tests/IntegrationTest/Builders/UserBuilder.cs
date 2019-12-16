using System;
using Security.Core.Entities;
using Security.Infrastructure.Helper;

namespace IntegrationTest.Builders
{
    public class UserBuilder
    {
        private User _user;
        public const string UserId = "testEmail";
        public const string UpdatedUserId = "UpdatedTestEmail";
        public const string RefreshToken = "1234";
        public UserBuilder()
        {

        }

        public User WithNoItems()
        {
            _user = new User { Email = UserId };
            return _user;
        }

        public User WithOneRefreshToken()
        {
            _user = new User(
                "firstName",
                "",
                "",
                UserId,
                true,
                null,
                DateTime.MaxValue,
                string.Empty,
                true);
            _user.AddRefreshToken(RefreshToken, Guid.NewGuid(), "127.0.0.1");
            return _user;
        }

        public User WithRoles()
        {
            _user = new User(
                "firstNameWithRole",
                "LastNameWithRole",
                UserId,
                UserId,
                true,
                null,
                DateTime.MaxValue,
                string.Empty,
                true);
            _user.AddRole(new Role[] { new Role(Roles.Admin), new Role(Roles.Doctor) });
            return _user;
        }

        public User UserToUpdate()
        {
            _user = new User(
                "UpdatedFirstNameWithRole",
                "UpdatedLastNameWithRole",
                UpdatedUserId,
                UpdatedUserId,
                true,
                null,
                DateTime.Today.AddDays(-1000),
                string.Empty,
                true);
            _user.AddRole(new Role(Roles.Admin), new Role(Roles.Patient));
            return _user;
        }

        public User WithOneRefreshTokenAndRoles()
        {
            _user = new User(
                "firstName",
                "",
                "",
                UserId,
                true,
                null,
                DateTime.MaxValue,
                string.Empty,
                true);
            _user.AddRefreshToken(RefreshToken, Guid.NewGuid(), "127.0.0.1");
            _user.AddRole(new Role[] { new Role(Roles.Admin), new Role(Roles.Doctor) });
            return _user;
        }
    }
}
