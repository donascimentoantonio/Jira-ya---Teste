using AutoMapper;
using Jira_ya.Application.DTOs;
using Jira_ya.Domain.Entities;

namespace Jira_ya.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Username))
                .ReverseMap()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Name));
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Name));
        }
    }
}
