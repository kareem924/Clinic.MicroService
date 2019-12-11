using System;
using Security.Core.Entities;
using Shouldly;
using Xunit;

namespace UnitTest.ApplicationCore.Entities
{
    public class UserTests
    {
        [Fact]
        public void HasValidRefreshToken_GivenValidToken_ShouldReturnTrue()
        {
            const string refreshToken = "a2/tXjsnTXOJejN+9M8OD+uwhMcTDoeqkKjCVc5hesQ=";
            var user = new User(
                "firstName", 
                "", 
                "", 
                "email", 
                true, 
                null, 
                DateTime.MaxValue, 
                string.Empty,
                true);
            user.AddRefreshToken(refreshToken, Guid.NewGuid(), "127.0.0.1");

            var result = user.HasValidRefreshToken(refreshToken);

            result.ShouldBeTrue();
        }

        [Fact]
        public void HasValidRefreshToken_GivenExpiredToken_ShouldReturnFalse()
        {
            const string refreshToken = "1234";
            var user = new User(
                "firstName", 
                "", 
                "", 
                "email", 
                true, 
                null, 
                DateTime.MaxValue,
                string.Empty,
                true);

            var result = user.HasValidRefreshToken(refreshToken);

            result.ShouldBeFalse();
        }

        [Fact]
        public void HasRole_GivenValidRole_ShouldReturnTrue()
        {
            var role = new Role("test");
            var user = new User(
                "firstName",
                "",
                "",
                "email",
                true,
                null,
                DateTime.MaxValue,
                string.Empty,
                true);
            user.AddRole(role);

            var result = user.HasRole(role);

            result.ShouldBeTrue();
        }
    }
}
