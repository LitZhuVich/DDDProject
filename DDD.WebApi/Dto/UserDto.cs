using DDD.Domain.ValueObjects;

namespace DDD.WebApi.Dto
{
    public class UserDto
    {
        public Guid Id { get; init; }
        public string UserName { get; set; } = string.Empty;

        public required PhoneNumber PhoneNumber;// 手机号
    }
}
