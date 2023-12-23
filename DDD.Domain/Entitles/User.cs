using DDD.Domain.ValueObjects;

namespace DDD.Domain.Entitles
{
    public record User : IAggregateRoot
    {
        public Guid Id { get; init; }
        public string? UserName { get; set; }
        public bool IsDeleted { get; set; }
        public PhoneNumber PhoneNumber { get; private set; } // 手机号

        public string passwordHash = ""; // 密码散列值
        public UserAccessFail UserAccessFail { get; private set; } // 用户状态
        private User() { }
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
        public bool HasPassword() => string.IsNullOrEmpty(passwordHash);

        /// <summary>
        /// 检查密码是否正确
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckPassword(string password) => passwordHash == HashHelper.ComputeMd5Hash(password);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">密码必须大于3</exception>
        public void ChangePassword(string password)
        {
            if (password.Length <= 3)
            {
                throw new ArgumentOutOfRangeException("密码必须大于3位");
            }
            passwordHash = HashHelper.ComputeMd5Hash(password);
        }

        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            if (phoneNumber.Tel.Length <= 8)
            {
                throw new ArgumentOutOfRangeException("手机号必须大于8位");
            }

            PhoneNumber = phoneNumber;
        }
    }
}
