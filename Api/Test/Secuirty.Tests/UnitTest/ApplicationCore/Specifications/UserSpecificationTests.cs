using System;
using System.Collections.Generic;
using System.Linq;
using Security.Core.Entities;
using Security.Core.Specification;
using Shouldly;
using Xunit;

namespace UnitTest.ApplicationCore.Specifications
{
    public class UserSpecificationTest
    {
        private readonly Guid _userId = Guid.NewGuid();

        [Fact]
        public void MatchesUserWithGivenId()
        {
            var spec = new UserSpecification(_userId);

            var result = GetTestUsersCollection()
                .AsQueryable()
                .FirstOrDefault(spec.Criteria);

            result.ShouldNotBeNull();
            result.Id.ShouldBeNull();

        }

        [Fact]
        public void MatchesNoBasketsIfIdNotPresent()
        {
            var badId = Guid.Empty;
            var spec = new UserSpecification(badId);

            GetTestUsersCollection()
                .AsQueryable()
                .Any(spec.Criteria).ShouldBeFalse();
        }


        private List<User> GetTestUsersCollection()
        {
            return new List<User>()
            {
                new User() { Id = Guid.NewGuid() },
                new User() { Id = Guid.NewGuid() },
                new User() { Id = _userId }
            };
        }
    }
}
