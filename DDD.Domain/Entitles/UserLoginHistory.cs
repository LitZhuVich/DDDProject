using DDD.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Entitles
{
    //因为有脱离User直接查一段时间内搜索人登陆记录的需求，所以这个时单独的聚合
    public record UserLoginHistory : IAggregateRoot
    {
        public long Id { get; init; }
        public Guid? UserId { get; init; }//用户Id
        public PhoneNumber? PhoneNumber { get; init; }//手机号
        public DateTime CreatedDateTime { get; init; }//时间
        public string? Messsage { get; init; }//消息
        private UserLoginHistory() { }
        public UserLoginHistory(Guid? userId,
            PhoneNumber phoneNumber, string message)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
            CreatedDateTime = DateTime.Now;
            Messsage = message;
        }
    }
}
