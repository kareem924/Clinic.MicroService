using Auth.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasOne<RefreshToken>(s => s.RefreshToken)
            //    .WithOne(ad => ad.User)
            //    .HasForeignKey<RefreshToken>(b => b.UserId);




        }
    }
}
