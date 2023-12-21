using DDD.Domain.Entitles;
using DDD.Domain.Result;

namespace DDD.Domain.Strategies
{
    /// <summary>
    /// 实现用户访问策略类
    /// </summary>
    public interface IUserAccessStrategy 
    {
        UserAccessResult CheckAccess(User user, string password);
    }
    /// <summary>
    /// 成功
    /// </summary>
    public class OkStrategy : IUserAccessStrategy
    {
        public UserAccessResult CheckAccess(User user, string password)
        {
            return UserAccessResult.Ok;
        }
    }
    /// <summary>
    /// 手机号不存在
    /// </summary>
    public class PhoneNumberNotFoundStrategy : IUserAccessStrategy
    {
        public UserAccessResult CheckAccess(User user, string password)
        {
            return UserAccessResult.PhoneNumberNotFound;
        }
    }
    /// <summary>
    /// 被锁定
    /// </summary>
    public class LockoutStrategy : IUserAccessStrategy
    {
        public UserAccessResult CheckAccess(User user, string password)
        {
            return UserAccessResult.Lockout;
        }
    }
    /// <summary>
    /// 没有密码
    /// </summary>
    public class NoPasswordStrategy : IUserAccessStrategy
    {
        public UserAccessResult CheckAccess(User user, string password)
        {
            return UserAccessResult.NoPassword;
        }
    }
    /// <summary>
    /// 密码错误
    /// </summary>
    public class PasswordErrorStrategy : IUserAccessStrategy
    {
        public UserAccessResult CheckAccess(User user, string password)
        {
            return UserAccessResult.PasswordError;
        }
    }
}
