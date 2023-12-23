using DDD.Domain.Entitles;
using DDD.Domain.Repository;
using DDD.Domain.Result;
using DDD.Domain.ResultEvents;
using DDD.Domain.Results;
using DDD.Domain.ValueObjects;

namespace DDD.Domain.Services
{
    public class UserDomainService
    {
        /// <summary>
        /// 注入仓储
        /// </summary>
        private readonly IUserDomainRepository _userDomainRepository;

        public UserDomainService(IUserDomainRepository userDomainRepository)
        {
            _userDomainRepository = userDomainRepository;
        }

        /// <summary>
        /// 重置错误信息
        /// </summary>
        /// <param name="user"></param>
        public static void ResetAccessFail(User? user) => user?.UserAccessFail.Reset();

        /// <summary>
        /// 是否被锁定
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsLockOut(User user) => user.UserAccessFail.IsLockOut();

        /// <summary>
        /// 处理一次“登陆失败”
        /// </summary>
        /// <param name="user"></param>
        public static void AccessFail(User? user) => user?.UserAccessFail.Fail();

        /// <summary>
        /// 检查手机号对应的密码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserAccessResult> CheckLoginAsync(PhoneNumber phoneNumber, string password)
        {
            UserAccessResult result;
            var user = await _userDomainRepository.FindOneAsync(phoneNumber);

            if (user == null) // 为空，手机号错误
            {
                result = UserAccessResult.PhoneNumberNotFound;
            }
            else if (IsLockOut(user)) // 被锁定
            {
                result = UserAccessResult.Lockout;
            }
            else if (user.HasPassword()) // 没有密码
            {
                result = UserAccessResult.NoPassword;
            }
            else if(user.CheckPassword(password)) // 密码正确
            {
                result = UserAccessResult.Ok;
            }
            else // 密码错误
            {
                result = UserAccessResult.PasswordError;
            }

            //如果密码正确则重置错误信息，否则处理一次“登陆失败”
            if (result == UserAccessResult.Ok)
            {
                ResetAccessFail(user);
            }
            else
            {
                AccessFail(user);
            }
            // 发布集成事件
            await _userDomainRepository.PublishEventAsync(new UserAccessResultEvent(phoneNumber, result));
            return result;
        }

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<CheckCodeResult> CheckPhoneNumberAsync(PhoneNumber phoneNumber,string code)
        {
            var user = await _userDomainRepository.FindOneAsync(phoneNumber);

            if (user == null) // 查找用户为空
            {
                return CheckCodeResult.PhoneNumberNotFound;
            }
            else if (user.UserAccessFail.IsLockOut()) // 被锁定
            {
                return CheckCodeResult.Lockout;
            }

            // 获取服务器验证码
            string? codeInServer = await _userDomainRepository.FindPhoneNumberCodeAsync(phoneNumber);

            if (codeInServer == null) // 服务器验证码为空
            {
                AccessFail(user);
                return CheckCodeResult.CodeError;
            }
            if (codeInServer == code) // 验证码与服务器验证码一致
            {
                return CheckCodeResult.Ok;
            }
            else // 不一致
            {
                AccessFail(user);
                return CheckCodeResult.CodeError;
            }
        }
    }
}
