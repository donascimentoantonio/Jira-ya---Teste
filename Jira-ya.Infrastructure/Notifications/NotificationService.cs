using Jira_ya.Domain.Interfaces;

namespace Jira_ya.Infrastructure.Notifications
{
    public class NotificationService : INotificationService
    {
        public void Notify(string message, int userId)
        {
            // Notificação simulada
        }
    }
}
