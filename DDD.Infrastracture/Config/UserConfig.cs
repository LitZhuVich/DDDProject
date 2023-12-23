using DDD.Domain.Entitles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Infrastructure.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 指定表名
            builder.ToTable("T_Users");
            // 配置PhoneNumber属性
            builder.OwnsOne(x => x.PhoneNumber, nb =>
            {
                // 设置属性Tel的最大长度为20，且不可为 unicode
                nb.Property(b => b.Tel).HasMaxLength(20).IsUnicode(false);
            });
            // 配置UserAccessFail属性
            builder.HasOne(b => b.UserAccessFail).WithOne(f => f.User)
                // 设置外键UserId
                .HasForeignKey<UserAccessFail>(f => f.UserId);
            // 设置passwordHash属性最大长度为100，且不可为 unicode
            builder.Property("passwordHash").HasMaxLength(100).IsUnicode(false);
            // 设置查询过滤器，只查询未删除的数据
            builder.HasQueryFilter(b => b.IsDeleted == false);
        }
    }
}