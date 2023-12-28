using DDD.Domain.Entitles;
using DDD.Domain.Repository;
using DDD.Domain.ResultEvents;
using DDD.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace DDD.Infrastructure.Repository
{
    public class UserDomainRepository : IUserDomainRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly MyDbContext _dbContext;
        private readonly IDistributedCache _cache; // 分布式缓存
        private readonly IMediator _mediator;

        public UserDomainRepository(MyDbContext dbContext, IDistributedCache cache, IMediator mediator, ConnectionMultiplexer redis)
        {
            _dbContext = dbContext;
            _cache = cache;
            _mediator = mediator;
            _redis = redis;
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

        public async Task<List<User>> FindAllAsync()
        {
            List<User> users = await _dbContext.Users.Include(u => u.UserAccessFail).ToListAsync();

            return users;
        }

        public async Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            User? user = await _dbContext.Users.Include(u => u.UserAccessFail).SingleOrDefaultAsync(ExpressionHelper.MakeEqual((User u) => u.PhoneNumber,phoneNumber));
            return user;
        }

        public async Task<User?> FindOneAsync(Guid userId)
        {
            User? user = await _dbContext.Users.Include(u => u.UserAccessFail).SingleOrDefaultAsync(u=>u.Id == userId);
            return user;
        }

        // IDistributedCache 对缓存的存储默认为其规定格式的Hash类型，虽然我们获取到的数据为string。这样我们想操作list、set等其他类型就不行了
        public async Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber)
        {
            string key = $"PhoneNumberCode_{phoneNumber.RegionNumber}_{phoneNumber.Tel}";

            string? code = await _redis.GetDatabase(1).StringGetAsync(key);
            await _redis.GetDatabase(1).KeyDeleteAsync(key);

            //string? code = await _cache.GetStringAsync(key); // 根据 key 获取验证码
            //_cache.Remove(key);// 删除 key
            return code;
        }

        public Task PublishEventAsync(UserAccessResultEvent userAccessResultEvent)
        {
            return _mediator.Publish(userAccessResultEvent);
        }

        public async Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber, string code)
        {
            string key = $"PhoneNumberCode_{phoneNumber.RegionNumber}_{phoneNumber.Tel}";
            Console.WriteLine(key);
            // 设置： key, value, 过期时间(5分钟 ( Redis 方法
            await _redis.GetDatabase(1).StringSetAsync(key, code, TimeSpan.FromMinutes(5));
            // 设置： key, value, 过期时间(5分钟
            //return _cache.SetStringAsync(key,code,new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            //});
        }
    }
}
