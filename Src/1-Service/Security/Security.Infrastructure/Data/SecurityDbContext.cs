using System;
using Common.General.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Security.Core.Entities;
using Security.Infrastructure.Data.Configurations;

namespace Security.Infrastructure.Data
{
    public class SecurityDbContext :
        IdentityDbContext<User, 
            Role,
            Guid, 
            IdentityUserClaim<Guid>,
            UserRole, 
            IdentityUserLogin<Guid>, 
            IdentityRoleClaim<Guid>, 
            IdentityUserToken<Guid>>
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
            : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=Clinic.Security;Integrated Security=true;Connect Timeout=90;Encrypt=False;TrustServerCertificate=True;");
            }

        }
        public SecurityDbContext()
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity<>).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }

            modelBuilder.ApplyConfiguration(new UserConfigure());
            base.OnModelCreating(modelBuilder);
        }

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseEntity<Guid>
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
