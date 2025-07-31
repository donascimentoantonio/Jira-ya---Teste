
using Jira_ya.Application.DTOs;
using Jira_ya.Application.Services.Interfaces;
using Jira_ya.Domain.Entities;
using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    using DomainTask = Jira_ya.Domain.Entities.Task;

    public class TaskService(ITaskRepository taskRepository, INotificationService notificationService) : ITaskService
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly INotificationService _notificationService = notificationService;

        public IEnumerable<TaskDto> GetAll()
        {
            return _taskRepository.GetAll().Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status
            });
        }

        public TaskDto GetById(int id)
        {
            var t = _taskRepository.GetById(id);
            if (t == null) return null;
            return new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status
            };
        }

        public TaskDto Create(TaskDto dto)
        {
            var entity = new DomainTask
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status
            };
            _taskRepository.Add(entity);
            _notificationService.Notify($"Tarefa criada: {entity.Title}", 0);
            dto.Id = entity.Id;
            return dto;
        }

        public TaskDto Update(int id, TaskDto dto)
        {
            var entity = _taskRepository.GetById(id);
            if (entity == null) return null;
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Status = dto.Status;
            _taskRepository.Update(entity);
            _notificationService.Notify($"Tarefa atualizada: {entity.Title}", 0);
            return dto;
        }

        public bool Delete(int id)
        {
            var entity = _taskRepository.GetById(id);
            if (entity == null) return false;
            _taskRepository.Delete(id);
            _notificationService.Notify($"Tarefa removida: {entity.Title}", 0);
            return true;
        }
    }
}
