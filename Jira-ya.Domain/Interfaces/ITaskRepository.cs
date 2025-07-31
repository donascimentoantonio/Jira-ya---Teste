namespace Jira_ya.Domain.Interfaces
{
    using DomainTask = Entities.DomainTask;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public interface ITaskRepository
    {
        Task<bool> AssignTaskAsync(Guid taskId, Guid userId);
        Task<DomainTask> GetByIdAsync(Guid id);
        Task<IEnumerable<DomainTask>> GetAllAsync();
        Task AddAsync(DomainTask task);
        Task UpdateAsync(DomainTask task);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<DomainTask>> GetTasksByUserIdAsync(Guid userId);
    }
}
