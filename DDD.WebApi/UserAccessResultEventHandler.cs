using DDD.Domain.Repository;
using DDD.Domain.ResultEvents;
using DDD.Infrastructure;
using DDD.WebApi.Attributes;
using MediatR;

namespace DDD.WebApi
{
    public class UserAccessResultEventHandler : INotificationHandler<UserAccessResultEvent>
    {
        private readonly IUserDomainRepository _userDomainRepository;

        public UserAccessResultEventHandler(IUserDomainRepository userDomainRepository)
        {
            _userDomainRepository = userDomainRepository;
        }

        [UnitOfWork(typeof(MyDbContext))]
        public async Task Handle(UserAccessResultEvent notification, CancellationToken cancellationToken)
        {
            await _userDomainRepository.AddNewLoginHistoryAsync(notification.PhoneNumber, $"登录结果是{notification.Result}");
        }
    }
}
