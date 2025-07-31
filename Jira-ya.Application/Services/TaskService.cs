using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    using DomainTask = Jira_ya.Domain.Entities.Task;

    public class TaskService : ITaskService    
    {
        private readonly ITaskRepository _taskRepository;
        private readonly INotificationService _notificationService;

        public TaskService(ITaskRepository taskRepository, INotificationService notificationService)
        {
            _taskRepository = taskRepository;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                AssignedUserId = t.AssignedUserId
            });
        }

        public async Task<TaskDto> GetByIdAsync(Guid id)
        {
            var t = await _taskRepository.GetByIdAsync(id);
            if (t == null) return null;
            return new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                AssignedUserId = t.AssignedUserId
            };
        }

        public async Task<TaskDto> CreateAsync(TaskDto dto)
        {
            var entity = new DomainTask
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Status = dto.Status,
                AssignedUserId = dto.AssignedUserId
            };
            await _taskRepository.AddAsync(entity);
            await _notificationService.NotifyAsync($"Tarefa criada: {entity.Title}", entity.AssignedUserId);
            return new TaskDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                Status = entity.Status,
                AssignedUserId = entity.AssignedUserId
            };
        }

        public async Task<TaskDto> UpdateAsync(Guid id, TaskDto dto)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            if (entity == null) return null;
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.DueDate = dto.DueDate;
            entity.Status = dto.Status;
            entity.AssignedUserId = dto.AssignedUserId;
            await _taskRepository.UpdateAsync(entity);
            await _notificationService.NotifyAsync($"Tarefa atualizada: {entity.Title}", entity.AssignedUserId);
            return new TaskDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                Status = entity.Status,
                AssignedUserId = entity.AssignedUserId
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            if (entity == null) return false;
            await _taskRepository.DeleteAsync(id);
            await _notificationService.NotifyAsync($"Tarefa removida: {entity.Title}", entity.AssignedUserId);
            return true;
        }

        public async Task<IEnumerable<TaskDto>> GetByUserIdAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                AssignedUserId = t.AssignedUserId
            });
        }
        public async Task<bool> AssignTaskAsync(Guid taskId, Guid userId)
        {
            var result = await _taskRepository.AssignTaskAsync(taskId, userId);
            if (result)
                await _notificationService.NotifyAsync($"Tarefa atribuída ao usuário {userId}", userId);
            return result;
        }
    }
}
