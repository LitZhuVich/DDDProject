using DDD.Domain.Entitles;
using DDD.Domain.ValueObjects;

namespace DDD.Domain.Repository
{
    public interface IUserDomainRepository
    {
        /// <summary>
        /// 根据手机号查找一个用户
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<User?> FindOneAsync(PhoneNumber phoneNumber);
        /// <summary>
        /// 根据ID查找一个用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User?> FindOneAsync(Guid userId);
        /// <summary>
        /// 添加手机号登陆信息
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg);
        //Task PublishEventAsync(UserAccessResultEvent eventData);
        /// <summary>
        /// 保存手机号对应验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber, string code);
        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber);
    }
}
