using System;
using Common.General.Specification;
using Security.Core.Entities;

namespace Security.Core.Specification
{
    public sealed class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(Guid id) : base(u => u.Id == id)
        {
            AddInclude(u => u.Roles);
            AddInclude(u => u.RefreshTokens);
        }
        public UserSpecification(string email) : base(u => u.Email == email)
        {
            AddInclude(u => u.Roles);
            AddInclude(u => u.RefreshTokens);
        }
    }
}
