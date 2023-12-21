using DDD.Domain.Entitles;
using DDD.Domain.Repository;
using DDD.Domain.Result;
using DDD.Domain.ValueObjects;

namespace DDD.Domain.Services
{
    public class UserDomainService
    {
        /// <summary>
        /// 注入仓储
        /// </summary>
        private readonly IUserDomainRepository _userDomainRepository;
        /// <summary>
        /// 注入防腐层发送短信接口
        /// </summary>
        private readonly ISmsCodeSender _smsSender;

        public UserDomainService(IUserDomainRepository userDomainRepository, ISmsCodeSender smsSender)
        {
            _userDomainRepository = userDomainRepository;
            _smsSender = smsSender;
        }

        public void ResetAccessFail(User? user)
        {
            user?.UserAccessFail.Reset();
        }

        public bool IsLockOut(User user)
        {
            return user.UserAccessFail.IsLockOut();
        }

        public void AccessFail(User? user)
        {
            user?.UserAccessFail.Fail();
        }

        public async Task<UserAccessResult> CheckPassword(PhoneNumber phoneNumber, string password)
        {
            UserAccessResult result;
            var user = await _userDomainRepository.FindOneAsync(phoneNumber);
            if (user == null)
            {
                result = UserAccessResult.PhoneNumberNotFound;
            }
            else if (IsLockOut(user))
            {
                result = UserAccessResult.Lockout;
            }
            else if (user.HasPassword())
            {
                result = UserAccessResult.NoPassword;
            }
            else if(user.CheckPassword(password))
            {
                result = UserAccessResult.OK;
            }
            else
            {
                result = UserAccessResult.PasswordError;
            }

            if (result == UserAccessResult.OK)
            {
                ResetAccessFail(user);
            }
            else
            {
                AccessFail(user);
            }

            return result;
        }
    }
}
