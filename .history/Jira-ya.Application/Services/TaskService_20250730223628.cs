using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly INotificationService _notificationService;

        public TaskService(ITaskRepository taskRepository, INotificationService notificationService)
        {
            _taskRepository = taskRepository;
            _notificationService = notificationService;
        }
    }
}
