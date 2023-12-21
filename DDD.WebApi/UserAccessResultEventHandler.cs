using DDD.Domain.Repository;
using DDD.Domain.ResultEvents;
using DDD.Infrastructure;
using MediatR;

namespace DDD.WebApi
{
    public class UserAccessResultEventHandler : INotificationHandler<UserAccessResultEvent>
    {
        private readonly IUserDomainRepository _userDomainRepository;
        private readonly MyDbContext _dbContext;
        public UserAccessResultEventHandler(IUserDomainRepository userDomainRepository, MyDbContext dbContext)
        {
            _userDomainRepository = userDomainRepository;
            _dbContext = dbContext;
        }

        public async Task Handle(UserAccessResultEvent notification, CancellationToken cancellationToken)
        {
            await _userDomainRepository.AddNewLoginHistoryAsync(notification.PhoneNumber, $"登录结果是{notification.Result}");
            await _dbContext.SaveChangesAsync();
        }
    }
}
