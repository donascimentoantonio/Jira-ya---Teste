namespace Jira_ya.Domain.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAsync(string message, Guid userId);
    }
}
