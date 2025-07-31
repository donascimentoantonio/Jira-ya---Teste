using Jira_ya.Application.Common;
using Jira_ya.Application.DTOs;
using System.Collections.Generic;

namespace Jira_ya.Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<bool> AssignTaskAsync(Guid taskId, Guid userId);
        Task<IEnumerable<TaskDto>> GetAllAsync();
        Task<TaskDto> GetByIdAsync(Guid id);
        Task<Result<TaskDto>> CreateAsync(CreateTaskRequest dto);
        Task<Result<TaskDto>> UpdateAsync(Guid id, TaskDto dto);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<IEnumerable<TaskDto>> GetByUserIdAsync(Guid userId);
    }
}
