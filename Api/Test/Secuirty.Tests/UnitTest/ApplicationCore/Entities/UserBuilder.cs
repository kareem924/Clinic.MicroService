using System;
using System.Collections.Generic;
using System.Text;
using Security.Core.Entities;

namespace UnitTest.ApplicationCore.Entities
{
    public class UserBuilder
    {
        private User _user;
        public string UserId = "testEmail";
        public const string refreshToken = "1234";
        public static readonly Role[] roles = new Role[] { new Role("") };

        public UserBuilder()
        {

        }

        public User WithNoItems()
        {
            _user = new User { Email = UserId };
            return _user;
        }

        public User WithDefaultRoles()
        {
            var defaultTestRoles = new List<Role>()
            {
                new Role("testRole1"),
                new Role("testRole2"),
                new Role("testRole3")
            }.ToArray();
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
            _user.AddRole(defaultTestRoles);
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
            _user.AddRefreshToken(refreshToken, Guid.NewGuid(), "127.0.0.1");
            return _user;
        }
    }
}
