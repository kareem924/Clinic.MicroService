using System;
using Auth.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure
{
    public class ClinicDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=localhost;Trusted_Connection=True;Initial Catalog=ClinicSecurity;Integrated Security=true;Connect Timeout=90;");
            }
            

        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    //builder.ApplyConfiguration(new UserConfiguration());
        //}
    }
}
