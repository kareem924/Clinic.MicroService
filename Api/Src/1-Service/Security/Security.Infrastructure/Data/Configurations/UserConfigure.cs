using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Core.Entities;

namespace Security.Infrastructure.Data.Configurations
{
    internal class UserConfigure : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasMany(u => u.Roles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.HasMany(r => r.RefreshTokens)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            builder.OwnsOne(m => m.Address, a =>
            {
                a.Property(p => p.Country).HasMaxLength(600)
                    .HasColumnName("Country")
                    .HasDefaultValue("");
                a.Property(p => p.City).HasMaxLength(150)
                    .HasColumnName("City")
                    .HasDefaultValue("");
                a.Property(p => p.State).HasMaxLength(60)
                    .HasColumnName("State")
                    .HasDefaultValue("");
                a.Property(p => p.Street).HasMaxLength(12)
                    .HasColumnName("Street")
                    .HasDefaultValue("");
            });
        }
    }
}
