using DDD.Domain.Result;
using DDD.Domain.ValueObjects;
using MediatR;

namespace DDD.Domain.ResultEvents
{
    public record UserAccessResultEvent(PhoneNumber PhoneNumber, UserAccessResult Result) : INotification;
}
