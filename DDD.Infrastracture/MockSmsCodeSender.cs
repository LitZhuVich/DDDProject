using DDD.Domain;
using DDD.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure
{
    public class MockSmsCodeSender : ISmsCodeSender
    {
        public Task SendCodeAsync(PhoneNumber phoneNumber, string code)
        {
            Console.WriteLine($"向{ phoneNumber.RegionNumber }-{ phoneNumber.Tel }发送验证码{ code }");
            return Task.CompletedTask;
        }
    }
}
