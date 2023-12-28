using DDD.Domain.Entitles;

namespace DDD.WebApi.Dto
{
    public class UserAccessFailDto
    {
        public Guid Id { get; init; }
        public User? User { get; init; } // 用户
        public Guid UserId { get; init; } // 用户ID

        public bool isLockOut; // 是否被锁定
        public DateTime? LockoutEnd { get; private set; } // 锁定结束日期
        public int AccessFailedCount { get; private set; } // 锁定次数
    }
}
