using AutoMapper;
using Jira_ya.Application.DTOs;
using Jira_ya.Domain.Entities;

namespace Jira_ya.Application.Mapping
{
    public class TaskProfile : Profile
    {

        public TaskProfile()
        {
            CreateMap<DomainTask, TaskDto>().ReverseMap();
            CreateMap<CreateTaskRequest, DomainTask>();
        }
    }
}
