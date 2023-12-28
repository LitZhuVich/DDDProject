namespace DDD.Domain.Result
{
    public enum UserAccessResult
    {
        /// <summary>
        /// 返回成功
        /// </summary>
        Ok,
        /// <summary>
        /// 手机号不存在
        /// </summary>
        PhoneNumberNotFound,
        /// <summary>
        /// 用户被锁定
        /// </summary>
        Lockout,
        /// <summary>
        /// 没有密码
        /// </summary>
        NoPassword,
        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordError
    }

}
