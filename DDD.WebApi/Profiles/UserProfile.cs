using AutoMapper;
using DDD.Domain.Entitles;
using DDD.WebApi.Dto;

namespace DDD.WebApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
