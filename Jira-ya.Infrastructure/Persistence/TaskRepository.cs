using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Infrastructure.Persistence
{
    using DomainTask = Jira_ya.Domain.Entities.Task;

    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class TaskRepository : ITaskRepository
        public async Task<bool> AssignTaskAsync(Guid taskId, Guid userId)
        {
            var task = await context.Tasks.FindAsync(taskId);
            if (task == null) return false;
            task.AssignedUserId = userId;
            await context.SaveChangesAsync();
            return true;
        }
    {
        private readonly AppDbContext context;
        public TaskRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<DomainTask>> GetTasksByUserIdAsync(Guid userId)
        {
            return await context.Tasks
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();
        }

        public async Task<DomainTask> GetByIdAsync(Guid id)
        {
            return await context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<DomainTask>> GetAllAsync()
        {
            return await context.Tasks.ToListAsync();
        }

        public async Task AddAsync(DomainTask task)
        {
            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DomainTask task)
        {
            context.Tasks.Update(task);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await context.Tasks.FindAsync(id);
            if (task != null)
            {
                context.Tasks.Remove(task);
                await context.SaveChangesAsync();
            }
        }
    }
}
