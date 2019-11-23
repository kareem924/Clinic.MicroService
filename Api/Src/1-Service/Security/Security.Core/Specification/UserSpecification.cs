using System;
using Common.General.Entity;
using Common.General.Specification;
using Security.Core.Entities;

namespace Security.Core.Specification
{
    public sealed class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(Guid id) : base(user => user.Id == id)
        {
            AddInclude(u => u.Roles);
            AddInclude("Roles.Role");
            AddInclude(u => u.RefreshTokens);

        }
        public UserSpecification(string email) : base(user => user.Email == email)
        {
            AddInclude(u => u.Roles);
            AddInclude("Roles.Role");
            AddInclude(u => u.RefreshTokens);
        }
        public UserSpecification(PagedQueryBase pageQuery, string email, string name)
            : base(user =>
                (string.IsNullOrEmpty(email) || user.Email.Contains(email)) ||
                (string.IsNullOrEmpty(name) || user.FullName.Contains(name)))
        {
            AddInclude(u => u.Roles);
            AddInclude("Roles.Role");
            ApplyPaging(pageQuery.Skip, pageQuery.PageSize);
        }
    }
}
