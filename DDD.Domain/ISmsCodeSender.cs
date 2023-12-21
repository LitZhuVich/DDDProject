using DDD.Domain.ValueObjects;

namespace DDD.Domain
{
    /// <summary>
    /// 防腐层接口
    /// </summary>
    public interface ISmsCodeSender
    {
        /// <summary>
        /// 根据手机号发送验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task SendCodeAsync(PhoneNumber phoneNumber, string code);
    }
}
