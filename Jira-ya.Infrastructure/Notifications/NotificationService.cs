using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        public Task NotifyAsync(string message, Guid userId)
        {
            // Notificação simulada
            return Task.CompletedTask;
        }
    }
}
