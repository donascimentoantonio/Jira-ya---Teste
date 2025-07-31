using Jira_ya.Application.DTOs;
using System.Collections.Generic;

namespace Jira_ya.Application.Services.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskDto> GetAll();
        TaskDto GetById(int id);
        TaskDto Create(TaskDto dto);
        TaskDto Update(int id, TaskDto dto);
        bool Delete(int id);
    }
}
