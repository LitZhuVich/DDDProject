using DDD.Domain;
using DDD.Domain.Repository;
using DDD.Domain.ValueObjects;
using DDD.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure
{
    public class MockSmsCodeSender : ISmsCodeSender
    {
        /// <summary>
        /// 注入仓储
        /// </summary>
        private readonly IUserDomainRepository _userDomainRepository;

        public MockSmsCodeSender(IUserDomainRepository userDomainRepository)
        {
            _userDomainRepository = userDomainRepository;
        }

        public async Task<string> SendCodeAsync(PhoneNumber phoneNumber)
        {
            string code = new Random().Next(10000, 99999).ToString();
            await _userDomainRepository.SavePhoneNumberCodeAsync(phoneNumber, code);
            Console.WriteLine($"向{ phoneNumber.RegionNumber }-{ phoneNumber.Tel }发送验证码{ code }");
            return code;
        }
    }
}
