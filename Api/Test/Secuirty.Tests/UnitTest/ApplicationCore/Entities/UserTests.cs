using System;
using System.Collections.Generic;
using System.Linq;
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

        [Theory]
        [InlineData("testRole11", "testRole1", "testRole2", "testRole3")]
        [InlineData("testRole1", "testRole9", "testRole12", "testRole15")]
        [InlineData("testRole4", "testRole55", "testRole5", "testRole7")]
        [InlineData("testRole1", "testRole2", "testRole3", "testRole4")]
        [InlineData("testRole1", "testRole2", "testRole3")]
        public void UpdatedRoles_GiveNewRoles_ShouldUpdatesOld(params string[] roleName)
        {
            var updatedRoles = roleName.Select(role => new Role(role)).ToArray();
            var user = new UserBuilder().WithDefaultRoles();

            user.UpdateRoles(updatedRoles);

            user.HasRole(updatedRoles[0]).ShouldBeTrue();
            user.HasRole(updatedRoles[1]).ShouldBeTrue();
            user.HasRole(updatedRoles[2]).ShouldBeTrue();
        }

    }
}
