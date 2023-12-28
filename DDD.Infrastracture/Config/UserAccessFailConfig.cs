using DDD.Domain.Entitles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Infrastructure.Config
{
    public class UserAccessFailConfig : IEntityTypeConfiguration<UserAccessFail>
    {
        public void Configure(EntityTypeBuilder<UserAccessFail> builder)
        {
            builder.ToTable("T_UserAccessFails");
            builder.Property("IsLockOut");

            // 设置查询过滤器，只查询未删除的数据
            builder.HasQueryFilter(b => b.IsDeleted == false);

            builder.HasIndex(x => x.UserId).HasDatabaseName("IX_UserId").IsUnique();
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_IsDeleted");
        }
    }
}
