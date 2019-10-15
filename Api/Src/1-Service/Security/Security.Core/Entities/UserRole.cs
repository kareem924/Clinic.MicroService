﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Security.Core.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}