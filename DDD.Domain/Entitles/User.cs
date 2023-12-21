using DDD.Domain.Helpers;
using DDD.Domain.ValueObjects;

namespace DDD.Domain.Entitles
{
    public record User
    {
        public Guid Id { get; init; }
        /// <summary>
        /// 手机号
        /// </summary>
        public PhoneNumber? PhoneNumber { get; private set; }
        /// <summary>
        /// 密码散列值
        /// </summary>
        public string passwordHash = "";
        /// <summary>
        /// 用户状态
        /// </summary>
        public UserAccessFail UserAccessFail { get; private set; }

        public User(PhoneNumber phoneNumber) 
        {
            PhoneNumber = phoneNumber;
            Id = Guid.NewGuid();
            UserAccessFail = new UserAccessFail(this);
        }
        /// <summary>
        /// 判断是否有密码
        /// </summary>
        /// <returns>bool</returns>
        public bool HasPassword()
        {
            return string.IsNullOrEmpty(passwordHash);
        }

        public bool CheckPassword(string password)
        {
            return passwordHash == HashHelper.ComputeMd5Hash(password);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">密码必须大于3</exception>
        public void ChangePassword(string password)
        {
            if (passwordHash.Length<=3)
            {
                throw new ArgumentOutOfRangeException("密码必须大于3");
            }
            passwordHash = HashHelper.ComputeMd5Hash(password);
        }
        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
