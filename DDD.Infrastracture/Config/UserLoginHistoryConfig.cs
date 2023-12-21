using DDD.Domain.Entitles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DDD.Infrastructure.Config
{
    public class UserLoginHistoryConfig : IEntityTypeConfiguration<UserLoginHistory>
    {
        public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
        {
            builder.ToTable("T_UserLoginHistories");
            builder.OwnsOne(x => x.PhoneNumber, nb =>
            {
                nb.Property(b => b.Tel).HasMaxLength(20).IsUnicode(false);
            });
        }
    }
}
