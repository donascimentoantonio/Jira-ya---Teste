using Jira_ya.Application.DTOs;
using System.Collections.Generic;

namespace Jira_ya.Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<bool> AssignTaskAsync(Guid taskId, Guid userId);
        Task<IEnumerable<TaskDto>> GetAllAsync();
        Task<TaskDto> GetByIdAsync(Guid id);
        Task<TaskDto> CreateAsync(TaskDto dto);
        Task<TaskDto> UpdateAsync(Guid id, TaskDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<TaskDto>> GetByUserIdAsync(Guid userId);
    }
}
