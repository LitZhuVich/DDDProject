namespace DDD.Domain.Entitles
{
    public record UserAccessFail
    {
        public Guid Id { get; init; }
        /// <summary>
        /// 用户
        /// </summary>
        public User? User { get; init; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; init; }
        /// <summary>
        /// 是否被锁定
        /// </summary>
        public bool isLockOut;
        /// <summary>
        /// 锁定结束日期
        /// </summary>
        public DateTime? LockoutEnd { get; private set; }
        /// <summary>
        /// 锁定次数
        /// </summary>
        public int AccessFailedCount { get; private set; }
        private UserAccessFail() { }
        public UserAccessFail(User user)
        {
            Id = Guid.NewGuid();
            User = user;
        }
        /// <summary>
        /// 重置用户状态
        /// </summary>
        public void Reset()
        {
            isLockOut = false;
            LockoutEnd = null;
            AccessFailedCount = 0;
        }
        /// <summary>
        /// 处理一次“登陆失败”
        /// </summary>
        public void Fail()
        {
            AccessFailedCount++;
            // 失败次数大于等于3则锁定，锁定时间为5分钟
            if (AccessFailedCount >= 3)
            {
                isLockOut = true;
                LockoutEnd = DateTime.Now.AddMinutes(5);
            }
        }
        /// <summary>
        /// 判断是否已锁定
        /// </summary>
        /// <returns></returns>
        public bool IsLockOut()
        {
            // 已锁定
            if (isLockOut)
            {
                // 已经过了锁定时间
                if (DateTime.Now > LockoutEnd)
                {
                    Reset();
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
