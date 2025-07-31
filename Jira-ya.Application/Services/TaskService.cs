using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Application.Services
{
    public class TaskService(ITaskRepository taskRepository, INotificationService notificationService)
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly INotificationService _notificationService = notificationService;
    }
}
