using DDD.Domain.Entitles;
using DDD.Domain.Repository;
using DDD.Domain.ResultEvents;
using DDD.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace DDD.Infrastructure.Repository
{
    public class UserDomainRepository : IUserDomainRepository
    {
        private readonly MyDbContext _dbContext;
        private readonly IDistributedCache _cache; // 分布式缓存
        private readonly IMediator _mediator;
        public UserDomainRepository(MyDbContext dbContext, IDistributedCache cache, IMediator mediator)
        {
            _dbContext = dbContext;
            _cache = cache;
            _mediator = mediator;
        }

        public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg)
        {
            User? user = await FindOneAsync(phoneNumber);
            Guid? userId = null;
            if (user != null)
            {
                userId = user.Id;
            }
            _dbContext.UserLoginHistories.Add(new UserLoginHistory(userId, phoneNumber, msg));
        }

        public async Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            User? user = await _dbContext.Users.Include(u=>u.UserAccessFail).SingleOrDefaultAsync(ExpressionHelper.MakeEqual((User u) => u.PhoneNumber,phoneNumber));
            return user;
        }

        public async Task<User?> FindOneAsync(Guid userId)
        {
            User? user = await _dbContext.Users.Include(u => u.UserAccessFail).SingleOrDefaultAsync(u=>u.Id == userId);
            return user;
        }

        public async Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber)
        {
            string key = $"PhoneNumberCode_{phoneNumber.RegionNumber}_{phoneNumber.Tel}";
            string? code = await _cache.GetStringAsync(key); // 根据key获取验证码
            _cache.Remove(key);// 删除key
            return code;
        }

        public Task PublishEventAsync(UserAccessResultEvent userAccessResultEvent)
        {
            return _mediator.Publish(userAccessResultEvent);
        }

        public Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber, string code)
        {
            string key = $"PhoneNumberCode_{phoneNumber.RegionNumber}_{phoneNumber.Tel}";
            // 设置： key, value, 过期时间(5分钟
            return _cache.SetStringAsync(key,code,new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }
    }
}
