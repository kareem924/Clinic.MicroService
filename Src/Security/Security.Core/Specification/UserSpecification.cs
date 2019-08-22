﻿using System;
using Common.General.Specification;
using Security.Core.Entities;

namespace Security.Core.Specification
{
    public sealed class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(Guid id) : base(u => u.Id == id)
        {
            AddInclude(u => u.Roles);
        }
    }
}
